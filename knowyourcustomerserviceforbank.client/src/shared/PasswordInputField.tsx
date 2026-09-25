import React from "react";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import { TextField, IconButton, InputAdornment } from "@mui/material";

export default function PasswordInputField(props: React.ComponentProps<typeof TextField>) {
  // State is scoped ONLY to this input
  const [showPassword, setShowPassword] = React.useState(false);

  return (
    <TextField
      {...props}
      type={showPassword ? "text" : "password"}
      slotProps={{
        input: {
          endAdornment: (
            <InputAdornment position="end">
              <IconButton
                aria-label="toggle password visibility"
                onClick={() => setShowPassword((prev) => !prev)}
                edge="start"
                color='primary'
              >
                {showPassword ? <VisibilityOff /> : <Visibility />}
              </IconButton>
            </InputAdornment>
          ),
        },
      }}
    />
  );
}