import { createBrowserRouter } from "react-router-dom";
import MainLayout from "../layouts/MainLayout";
import EventsPage from "../pages/EventsPage";
import EventDetailPage from "../pages/EventDetailPage";
import DiscussionsPage from "../pages/DiscussionsPage";
import DiscussionDetailPage from "../pages/DiscussionDetailPage";
import MembersPage from "../pages/MembersPage";
import MoviesPage from "../pages/MoviesPage";
import MovieDetailPage from "../pages/MovieDetailPage";
import VenuesPage from "../pages/VenuesPage";
import VenueDetailPage from "../pages/VenueDetailPage";
import AttendeeRecordsPage from "../pages/AttendeeRecordsPage";
import AttendancePage from "../pages/AttendancePage";
import DashboardPage from "../pages/DashboardPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <MainLayout />,
    children: [
      { index: true, element: <DashboardPage /> },
      { path: "events", element: <EventsPage /> },
      { path: "events/:id", element: <EventDetailPage /> },
      { path: "events/:id/attendance", element: <AttendancePage /> },
      { path: "discussions", element: <DiscussionsPage /> },
      { path: "discussions/:id", element: <DiscussionDetailPage /> },
      { path: "members", element: <MembersPage /> },
      { path: "movies", element: <MoviesPage /> },
      { path: "movies/:id", element: <MovieDetailPage /> },
      { path: "venues", element: <VenuesPage /> },
      { path: "venues/:id", element: <VenueDetailPage /> },
      { path: "attendees", element: <AttendeeRecordsPage /> },

    ],
  },
]);