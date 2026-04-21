import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { venuesApi } from "../api/services";
import type { Venue, ApiResponse } from "../types";

export default function VenueDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [venue, setVenue] = useState<Venue | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => { fetchVenue(); }, [id]);

  const fetchVenue = () => {
    setLoading(true);
    venuesApi
      .getById(id!)
      .then((res) => {
        const body: ApiResponse<Venue> = res.data;
        setVenue(body.data);
      })
      .catch(() => setError("Failed to load venue."))
      .finally(() => setLoading(false));
  };

  const groupedEvents = venue?.events.reduce((groups, event) => {
    const date = new Date(event.date).toLocaleDateString("en-US", {
      weekday: "long", year: "numeric", month: "long", day: "numeric",
    });
    if (!groups[date]) groups[date] = [];
    groups[date].push(event);
    return groups;
  }, {} as Record<string, NonNullable<typeof venue>["events"]>) ?? {};

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;
  if (!venue) return <p>Venue not found.</p>;

  return (
    <div>
      <button className="back-btn" onClick={() => navigate("/venues")}>← Back to Venues</button>

      <h1 className="detail-title">{venue.name}</h1>

      <div className="detail-meta">
        <p><strong>Address:</strong> {venue.address}</p>
        <p><strong>Capacity:</strong> {venue.capacity}</p>
      </div>

      <hr className="detail-divider" />

      <div className="section-header">
        <h3>Events</h3>
      </div>

      {!venue.events || venue.events.length === 0 ? (
        <div className="empty-state">No events at this venue.</div>
      ) : (
        <div style={{ display: "flex", flexDirection: "column", gap: 24 }}>
          {Object.entries(groupedEvents).map(([date, events]) => (
            <div key={date}>
              <p style={{
                fontSize: 12,
                fontWeight: 500,
                textTransform: "uppercase",
                letterSpacing: "0.08em",
                color: "var(--text-muted)",
                marginBottom: 10,
              }}>
                {date}
              </p>
              <div style={{ display: "flex", flexDirection: "column", gap: 8 }}>
                {events.map((event) => (
                  <div
                    key={event.id}
                    className="card"
                    onClick={() => navigate(`/events/${event.id}`)}
                    style={{ cursor: "pointer" }}
                  >
                    <p style={{ fontWeight: 600, fontSize: 15 }}>{event.title}</p>
                    <p style={{ fontSize: 13, color: "var(--text-muted)", marginTop: 4 }}>
                      {event.description}
                    </p>
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}