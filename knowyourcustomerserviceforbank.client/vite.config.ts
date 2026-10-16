import fs from 'fs';
import path from 'path';
import process, { env } from 'process';
import react from '@vitejs/plugin-react';
import child_process from 'child_process';
import tailwindcss from '@tailwindcss/vite';
import { devtools } from "@tanstack/devtools-vite";
import { tanstackRouter } from '@tanstack/router-vite-plugin';
import { defineConfig, loadEnv, lazyPlugins } from 'vite-plus';

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
    fmt: {},
    lint: {
      "plugins": [
        "oxc",
        "typescript",
        "unicorn"
      ],
      "categories": {
        "correctness": "warn"
      },
      "env": {
        "builtin": true
      },
      "ignorePatterns": [
        "dist"
      ],
      "overrides": [
        {
          "files": [
            "**/*.{ts,tsx}"
          ],
          "rules": {
            "constructor-super": "error",
            "for-direction": "error",
            "getter-return": "error",
            "no-async-promise-executor": "error",
            "no-case-declarations": "error",
            "no-class-assign": "error",
            "no-compare-neg-zero": "error",
            "no-cond-assign": "error",
            "no-const-assign": "error",
            "no-constant-binary-expression": "error",
            "no-constant-condition": "error",
            "no-control-regex": "error",
            "no-debugger": "error",
            "no-delete-var": "error",
            "no-dupe-class-members": "error",
            "no-dupe-else-if": "error",
            "no-dupe-keys": "error",
            "no-duplicate-case": "error",
            "no-empty": "error",
            "no-empty-character-class": "error",
            "no-empty-pattern": "error",
            "no-empty-static-block": "error",
            "no-ex-assign": "error",
            "no-extra-boolean-cast": "error",
            "no-fallthrough": "error",
            "no-func-assign": "error",
            "no-global-assign": "error",
            "no-import-assign": "error",
            "no-invalid-regexp": "error",
            "no-irregular-whitespace": "error",
            "no-loss-of-precision": "error",
            "no-misleading-character-class": "error",
            "no-new-native-nonconstructor": "error",
            "no-nonoctal-decimal-escape": "error",
            "no-obj-calls": "error",
            "no-prototype-builtins": "error",
            "no-redeclare": "error",
            "no-regex-spaces": "error",
            "no-self-assign": "error",
            "no-setter-return": "error",
            "no-shadow-restricted-names": "error",
            "no-sparse-arrays": "error",
            "no-this-before-super": "error",
            "no-unassigned-vars": "error",
            "no-undef": "error",
            "no-unexpected-multiline": "error",
            "no-unreachable": "error",
            "no-unsafe-finally": "error",
            "no-unsafe-negation": "error",
            "no-unsafe-optional-chaining": "error",
            "no-unused-labels": "error",
            "no-unused-private-class-members": "error",
            "no-unused-vars": "error",
            "no-useless-assignment": "error",
            "no-useless-backreference": "error",
            "no-useless-catch": "error",
            "no-useless-escape": "error",
            "no-with": "error",
            "preserve-caught-error": "error",
            "require-yield": "error",
            "use-isnan": "error",
            "valid-typeof": "error",
            "no-array-constructor": "error",
            "no-unused-expressions": "error",
            "typescript/ban-ts-comment": "error",
            "typescript/no-duplicate-enum-values": "error",
            "typescript/no-empty-object-type": "error",
            "typescript/no-explicit-any": "error",
            "typescript/no-extra-non-null-assertion": "error",
            "typescript/no-misused-new": "error",
            "typescript/no-namespace": "error",
            "typescript/no-non-null-asserted-optional-chain": "error",
            "typescript/no-require-imports": "error",
            "typescript/no-this-alias": "error",
            "typescript/no-unnecessary-type-constraint": "error",
            "typescript/no-unsafe-declaration-merging": "error",
            "typescript/no-unsafe-function-type": "error",
            "typescript/no-wrapper-object-types": "error",
            "typescript/prefer-as-const": "error",
            "typescript/prefer-namespace-keyword": "error",
            "typescript/triple-slash-reference": "error",
            "react/rules-of-hooks": "error",
            "react/exhaustive-deps": "warn",
            "react/static-components": "error",
            "react/use-memo": "error",
            "react/preserve-manual-memoization": "error",
            "react/incompatible-library": "warn",
            "react/immutability": "error",
            "react/globals": "error",
            "react/refs": "error",
            "react/set-state-in-effect": "error",
            "react/error-boundaries": "error",
            "react/purity": "error",
            "react/set-state-in-render": "error",
            "react/unsupported-syntax": "warn",
            "react/no-children-prop": "allow",
            "react/only-export-components": [
              "error",
              {
                "allowConstantExport": true,
                "allowCompoundComponents": true
              }
            ]
          },
          "plugins": [
            "react"
          ],
          "env": {
            "browser": true
          }
        },
        {
          "files": [
            "src/routes/**/*.{ts,tsx}"
          ],
          "rules": {
            "react/only-export-components": "off"
          },
          "plugins": [
            "react"
          ]
        }
      ],
      "options": {
        "typeAware": true,
        "typeCheck": true
      },
      "jsPlugins": [
        {
          "name": "vite-plus",
          "specifier": "vite-plus/oxlint-plugin"
        }
      ],
      "rules": {
        "vite-plus/prefer-vite-plus-imports": "error"
      }
    },
    plugins: lazyPlugins(() => [
      devtools({
        enhancedLogs: { enabled: true },
        logging: true,
        removeDevtoolsOnBuild: true,
      }),
      tanstackRouter({
        target: 'react',
        autoCodeSplitting: true,
      }),
      react({ compiler: true }),
      tailwindcss()
    ]),
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
