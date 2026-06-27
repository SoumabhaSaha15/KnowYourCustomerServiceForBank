import { createFileRoute } from "@tanstack/react-router";
import axios from "axios";

import {
  Box,
  CircularProgress,
  Container,
  // Link,
  Card,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";

interface Forecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

const api = axios.create({
  baseURL: "/",
});
export const Route = createFileRoute("/")({
  loader: async () => {
    const { data } = await api.get<Forecast[]>("weatherforecast");
    return data;
  },
  pendingComponent: () => (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        py: 5,
      }}
    >
      <CircularProgress />
    </Box>
  ),
  component: WeatherCard,
});

function WeatherCard() {
  const forecasts = Route.useLoaderData();

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Typography
        variant="h4"
        component="h1"
        id="tableLabel"
        sx={{ fontWeight: 700, mb: 1 }}
      >
        Weather Forecast
      </Typography>

      <Typography
        variant="body1"
        color="text.secondary"
        sx={{ mb: 3 }}
      >
        This component demonstrates fetching data from the server.
      </Typography>

      <TableContainer component={Card} sx={{
        borderRadius: 2, boxShadow: 2,
        transition: "box-shadow 200ms ease-in-out",
        "&:hover": {
          boxShadow: 16,
        },
      }}>
        <Table aria-labelledby="tableLabel">
          <TableHead>
            <TableRow sx={{ "& th": { fontWeight: 700 } }}>
              <TableCell>Date</TableCell>
              <TableCell align="right">Temp. (°C)</TableCell>
              <TableCell align="right">Temp. (°F)</TableCell>
              <TableCell>Summary</TableCell>
            </TableRow>
          </TableHead>

          <TableBody>
            {forecasts.map((forecast) => (
              <TableRow key={forecast.date}>
                <TableCell>{forecast.date}</TableCell>
                <TableCell align="right">
                  {forecast.temperatureC}
                </TableCell>
                <TableCell align="right">
                  {forecast.temperatureF}
                </TableCell>
                <TableCell>{forecast.summary}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
}
