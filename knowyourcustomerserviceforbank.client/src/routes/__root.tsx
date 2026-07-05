import * as React from 'react';
import { TanStackDevtools } from '@tanstack/react-devtools';
import { Outlet, createRootRoute } from '@tanstack/react-router';
import { FormDevtoolsPanel } from '@tanstack/react-form-devtools';
import { TanStackRouterDevtools } from "@tanstack/router-devtools";

export const Route = createRootRoute({
  component: RootComponent,
})

function RootComponent() {
  return (
    <React.Fragment>
      <Outlet />
      <TanStackDevtools plugins={[
        {
          name: "tanstack form",
          render: <FormDevtoolsPanel />,
          defaultOpen: true
        },
        {
          name: "tanstack router",
          render: <TanStackRouterDevtools position='bottom-right' />,
          defaultOpen: false
        }
      ]} />
    </React.Fragment>
  )
}
