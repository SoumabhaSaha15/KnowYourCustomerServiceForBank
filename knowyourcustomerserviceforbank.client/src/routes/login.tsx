// import React from 'react';
import base from "@/utils/axios-base";
import { useForm } from '@tanstack/react-form';
import { createFileRoute } from '@tanstack/react-router';
import {
  Container,
  Box,
  TextField,
  Button,
  Typography,
  Stack,
  CircularProgress
} from '@mui/material';
import { Login } from '@mui/icons-material'
import { userLogin } from "@/validators/user";
import PasswordInputField from '@/shared/PasswordInputField';
export const Route = createFileRoute('/login')({
  component: RouteComponent,
})

function RouteComponent() {

  const form = useForm({
    validators: { onChange: userLogin },
    onSubmit: async ({ value }) => {
      try {
        base.post('/Auth', value).then(console.dir);
      } catch (error) {
        console.error(error);
      }
    }
  });

  return (
    <Container maxWidth="sm">
      <Box className="flex flex-col justify-center min-h-screen">
        <Stack
          className='hover:scale-105 p-2 rounded-lg border-2'
          spacing={4}
          sx={{
            borderColor: (theme) => theme.palette.divider,
            backgroundColor:
              (theme) => theme.palette.background.paper,
          }}
          component="form"
          method="post"
          onSubmit={(e) => {
            e.preventDefault();
            e.stopPropagation();
            form.handleSubmit();
          }}
        >
          <Typography
            variant='h5'
            component="h5"
            sx={{ borderColor: "divider", backgroundColor: "secondary.main", color: "secondary.contrastText" }}
            className='w-full max-w-160 p-2 rounded-md text-center font-black h-14'
            children={"Login"}
            gutterBottom
          />

          <form.Field name="email">
            {(field) => {
              return (
                <TextField
                  label={field.name}
                  name={field.name}
                  type="email"
                  value={field.state.value}
                  onChange={(e) => field.setValue(e.target.value)}
                  fullWidth
                  variant="outlined"
                  error={!!field.state.meta.errors.length}
                  helperText={field.state.meta.errors.shift()?.message}
                />
              )
            }}
          </form.Field>

          <form.Field name="password">
            {(field) => {
              return (
                <PasswordInputField
                  label={field.name}
                  name={field.name}
                  value={field.state.value}
                  onChange={(e) => field.setValue(e.target.value)}
                  fullWidth
                  variant="outlined"
                  error={!!field.state.meta.errors.length}
                  helperText={field.state.meta.errors.shift()?.message}
                />
              )
            }}
          </form.Field>

          <form.Subscribe selector={(state) => [state.canSubmit, state.isSubmitting]}>
            {([canSubmit, isSubmitting]) => (
              <Button
                variant="contained"
                type="submit"
                className='h-12 rounded-md'
                fullWidth
                startIcon={<Login />}
                disabled={!canSubmit}
              > {isSubmitting && <CircularProgress aria-label="Loading…" />}
                Log In
              </Button>
            )}
          </form.Subscribe>
        </Stack>
      </Box>
    </Container>);
}



