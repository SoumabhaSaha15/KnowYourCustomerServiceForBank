import * as React from 'react';
import { Slide } from '@mui/material';
import { TransitionGroup } from "react-transition-group";
import { TanStackDevtools } from '@tanstack/react-devtools';
import { FormDevtoolsPanel } from '@tanstack/react-form-devtools';
import { ReactQueryDevtoolsPanel } from '@tanstack/react-query-devtools';
import { Outlet, createRootRoute, useLocation } from '@tanstack/react-router';
import { TanStackRouterDevtoolsPanel } from "@tanstack/react-router-devtools";

export const Route = createRootRoute({
  component: RootComponent,
})

function RootComponent() {
  const location = useLocation();
  return (
    <React.Fragment>
      <TransitionGroup className="page-transition-group">
        <Slide
          direction='right'
          key={location.pathname}
          timeout={{ enter: 500 }}
          unmountOnExit
        >
          <div>
            <Outlet />
          </div>
        </Slide>
      </TransitionGroup>
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
