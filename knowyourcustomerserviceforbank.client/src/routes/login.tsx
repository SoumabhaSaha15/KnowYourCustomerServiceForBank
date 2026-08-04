import React from 'react';
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
} from '@mui/material';
import { Login, Visibility, VisibilityOff } from '@mui/icons-material'
export const Route = createFileRoute('/login')({
  component: RouteComponent,
})

function RouteComponent() {

  const [passwordVisible, setPasswordVisible] = React.useState<"text" | "password">("password");

  const form = useForm({
    defaultValues: {
      email: 'admin@kycflow.com',
      password: 'admin123',
    },
    onSubmit: async ({ value }) => {
      try {
        base.get('/UserLogin').then((data: unknown) => console.log(data));
        const response = await base.post('/UserLogin', value);
        console.log(response.data, response.status);
      } catch (error) {
        console.error(error);
      }
    }
  });

  return (
    <Container maxWidth="sm">
      <Box
        className="flex flex-col justify-center min-h-screen"
      >
        <Typography variant="h4" component="h1" gutterBottom>
          Login
        </Typography>

        <Stack spacing={2} component="form" method="post" onSubmit={(e) => {
          e.preventDefault();
          e.stopPropagation();
          form.handleSubmit();
        }} >
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
                  helperText={field.state.meta.errors.shift()}
                />
              )
            }}
          </form.Field>

          <form.Field name="password">
            {(field) => {
              return (
                <TextField
                  label={field.name}
                  name={field.name}
                  value={field.state.value}
                  type={passwordVisible}
                  slotProps={{
                    input: {
                      endAdornment: (
                        <Button sx={{ color: 'primary.main', ":hover": { backgroundColor: 'transparent' } }} disableRipple onClick={() => {
                          setPasswordVisible(passwordVisible === "password" ? "text" : "password");
                        }}>
                          {passwordVisible === "password" ? <Visibility /> : <VisibilityOff />}
                        </Button>
                      )
                    }
                  }}
                  onChange={(e) => field.setValue(e.target.value)}
                  fullWidth
                  variant="outlined"
                  helperText={field.state.meta.errors.shift()}
                />
              )
            }}
          </form.Field>

          <Button variant="contained" type="submit" fullWidth startIcon={<Login />}>
            Sign In
          </Button>
        </Stack>
      </Box>
    </Container>);
}
