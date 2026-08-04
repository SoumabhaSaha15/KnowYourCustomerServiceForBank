import * as React from 'react';
import { TanStackDevtools } from '@tanstack/react-devtools';
import { Outlet, createRootRoute } from '@tanstack/react-router';
import { FormDevtoolsPanel } from '@tanstack/react-form-devtools';
import { ReactQueryDevtoolsPanel } from '@tanstack/react-query-devtools';
import { TanStackRouterDevtoolsPanel } from "@tanstack/react-router-devtools";

export const Route = createRootRoute({
  component: RootComponent,
})

function RootComponent() {
  return (
    <React.Fragment>
      <Outlet />
      <TanStackDevtools plugins={[
        {
          name: "Tanstack form",
          render: <FormDevtoolsPanel />,
          defaultOpen: true
        },
        {
          name: "Tanstack query",
          render: <ReactQueryDevtoolsPanel />,
          defaultOpen: false
        },
        {
          name: "Tanstack router",
          render: <TanStackRouterDevtoolsPanel />,
          defaultOpen: false
        }
      ]} />
    </React.Fragment>
  )
}
