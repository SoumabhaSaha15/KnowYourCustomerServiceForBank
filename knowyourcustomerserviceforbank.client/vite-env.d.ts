/// <reference types="vite/client" />
interface ImportMetaEnv {
  readonly DEV_SERVER_PORT: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}