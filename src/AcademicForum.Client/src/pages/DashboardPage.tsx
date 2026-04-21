import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { eventsApi, moviesApi, membersApi, venuesApi } from "../api/services";
import type { ApiResponse, Event, Movie, Member, Venue } from "../types";

interface StatCard {
  label: string;
  count: number;
  to: string;
  description: string;
}

export default function DashboardPage() {
  const navigate = useNavigate();
  const [stats, setStats] = useState<StatCard[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Promise.all([
      eventsApi.getAll(),
      moviesApi.getAll(),
      membersApi.getAll(),
      venuesApi.getAll(),
    ]).then(([eventsRes, moviesRes, membersRes, venuesRes]) => {
      const events: ApiResponse<Event[]> = eventsRes.data;
      const movies: ApiResponse<Movie[]> = moviesRes.data;
      const members: ApiResponse<Member[]> = membersRes.data;
      const venues: ApiResponse<Venue[]> = venuesRes.data;

      setStats([
        {
          label: "Events",
          count: events.data.length,
          to: "/events",
          description: "Academic forum events",
        },
        {
          label: "Movies",
          count: movies.data.length,
          to: "/movies",
          description: "Films in the program",
        },
        {
          label: "Members",
          count: members.data.filter((m) => m.role !== "Administrator").length,
          to: "/members",
          description: "Registered forum members",
        },
        {
          label: "Venues",
          count: venues.data.length,
          to: "/venues",
          description: "Available event venues",
        },
      ]);
    })
    .finally(() => setLoading(false));
  }, []);

  if (loading) return <p>Loading...</p>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h2>Dashboard</h2>
          <p>Welcome to the Academic Forum management panel.</p>
        </div>
      </div>

      <div style={{
        display: "grid",
        gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))",
        gap: 20,
      }}>
        {stats.map((stat) => (
          <div
            key={stat.label}
            className="card"
            onClick={() => navigate(stat.to)}
            style={{
              cursor: "pointer",
              transition: "box-shadow 0.15s, border-color 0.15s",
              padding: "28px 24px",
            }}
            onMouseEnter={(e) => {
              (e.currentTarget as HTMLDivElement).style.boxShadow = "0 4px 12px rgba(0,0,0,0.1)";
              (e.currentTarget as HTMLDivElement).style.borderColor = "var(--accent-light)";
            }}
            onMouseLeave={(e) => {
              (e.currentTarget as HTMLDivElement).style.boxShadow = "";
              (e.currentTarget as HTMLDivElement).style.borderColor = "";
            }}
          >
            <p style={{
              fontSize: 11,
              fontWeight: 500,
              textTransform: "uppercase",
              letterSpacing: "0.1em",
              color: "var(--text-muted)",
              marginBottom: 12,
            }}>
              {stat.label}
            </p>
            <p style={{
              fontFamily: "Lora, serif",
              fontSize: 48,
              fontWeight: 600,
              color: "var(--navy)",
              lineHeight: 1,
              marginBottom: 12,
            }}>
              {stat.count}
            </p>
            <p style={{
              fontSize: 13,
              color: "var(--text-secondary)",
            }}>
              {stat.description}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
}