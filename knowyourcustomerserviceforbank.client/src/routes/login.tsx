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
  IconButton,
  InputAdornment,
  CircularProgress
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
        base.post('/Auth', value).then(({ data }) => console.log(data));
      } catch (error) {
        console.error(error);
      }
    }
  });

  return (
    <Container maxWidth="sm">
      <Box
        className="flex flex-col justify-center min-h-screen"
      // sx={{ backgroundColor: theme => theme.palette.background.paper }}
      >
        <Stack
          spacing={3}
          sx={{
            padding: 1,
            borderRadius: 1,
            borderColor: (theme) => theme.palette.divider,
            // borderColor: (theme) => theme.palette.divider,
            borderWidth: 1,
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
            className='w-full max-w-160 p-2 rounded-xl text-center font-black h-14'
            children={"Login"}
            gutterBottom
          />
          {/* <Divider /> */}

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
                        <InputAdornment position="start">
                          <IconButton
                            className="transition-all"
                            edge="end"
                            color='primary'
                            sx={{ borderRadius: 1 }}
                            onClick={() => {
                              setPasswordVisible(passwordVisible === "password" ? "text" : "password");
                            }}
                            children={passwordVisible === "password" ? <Visibility /> : <VisibilityOff />}
                          />
                        </InputAdornment >
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

          <form.Subscribe selector={(state) => [state.canSubmit, state.isSubmitting]}>
            {([canSubmit, isSubmitting]) => (

              <Button
                variant="contained"
                type="submit"
                className='h-12'
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
