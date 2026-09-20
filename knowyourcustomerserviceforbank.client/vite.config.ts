import fs from 'fs';
import path from 'path';
import { env } from 'process';
import child_process from 'child_process';
import babel from "@rolldown/plugin-babel";
import tailwindcss from '@tailwindcss/vite';
import { defineConfig, loadEnv } from 'vite';
import { devtools } from "@tanstack/devtools-vite";
import { tanstackRouter } from '@tanstack/router-vite-plugin';
import react, { reactCompilerPreset } from '@vitejs/plugin-react';

const baseFolder =
  env.APPDATA !== undefined && env.APPDATA !== ''
    ? `${env.APPDATA}/ASP.NET/https`
    : `${env.HOME}/.aspnet/https`;

const certificateName = "knowyourcustomerserviceforbank.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(baseFolder)) {
  fs.mkdirSync(baseFolder, { recursive: true });
}

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
  if (0 !== child_process.spawnSync('dotnet', [
    'dev-certs',
    'https',
    '--export-path',
    certFilePath,
    '--format',
    'Pem',
    '--no-password',
  ], { stdio: 'inherit', }).status) {
    throw new Error("Could not create certificate.");
  }
}

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7016';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const loaded_env = loadEnv(mode, process.cwd(), '');
  return {
    plugins: [
      devtools({
        enhancedLogs: { enabled: true },
        logging: true,
        removeDevtoolsOnBuild: true,
      }),
      tanstackRouter({
        target: 'react',
        autoCodeSplitting: true,
      }), react(),
      babel({
        presets: [reactCompilerPreset()],
      }),
      tailwindcss()
    ],
    resolve: {
      alias: {
        '@': path.resolve('./src'),
      }
    },
    server: {
      proxy: {
        '^/api': {
          target,
          secure: false,
          rewrite: (path) => path.replace(/^\/api/, ""),
        }
      },
      port: parseInt(loaded_env.DEV_SERVER_PORT),
      https: {
        key: fs.readFileSync(keyFilePath),
        cert: fs.readFileSync(certFilePath),
      }
    }
  }
})
