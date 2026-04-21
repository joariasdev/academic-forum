import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { moviesApi } from "../api/services";
import type { Movie, ApiResponse } from "../types";
import ConfirmModal from "../components/ConfirmModal";

export default function MovieDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [movie, setMovie] = useState<Movie | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [confirmDeleteDiscussion, setConfirmDeleteDiscussion] = useState<number | null>(null);

  useEffect(() => { fetchMovie(); }, [id]);

  const fetchMovie = () => {
    setLoading(true);
    moviesApi
      .getById(id!)
      .then((res) => {
        const body: ApiResponse<Movie> = res.data;
        setMovie(body.data);
      })
      .catch(() => setError("Failed to load movie."))
      .finally(() => setLoading(false));
  };

  const doDeleteDiscussion = () => {
    if (!confirmDeleteDiscussion) return;
    import("../api/services").then(({ discussionsApi }) => {
      discussionsApi
        .delete(String(confirmDeleteDiscussion))
        .then(() => { setConfirmDeleteDiscussion(null); fetchMovie(); })
        .catch(() => setError("Failed to delete discussion."));
    });
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;
  if (!movie) return <p>Movie not found.</p>;

  return (
    <div>
      <button className="back-btn" onClick={() => navigate("/movies")}>← Back to Movies</button>

      <h1 className="detail-title">{movie.title}</h1>

      <div className="detail-meta">
        <p><strong>Director:</strong> {movie.director}</p>
        <p><strong>Genre:</strong> {movie.genre}</p>
        <p><strong>Release Date:</strong> {new Date(movie.releaseDate).toLocaleDateString()}</p>
        <p><strong>Synopsis:</strong> {movie.sinopsis}</p>
      </div>

      <hr className="detail-divider" />

      <div className="section-header">
        <h3>Discussions</h3>
      </div>

      {!movie.discussions || movie.discussions.length === 0 ? (
        <div className="empty-state">No discussions yet.</div>
      ) : (
        <div style={{ display: "flex", flexDirection: "column", gap: 12 }}>
          {movie.discussions.map((d) => (
            <div key={d.id} className="discussion-card">
              <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start" }}>
                <p
                  className="discussion-card-title"
                  onClick={() => navigate(`/discussions/${d.id}`, { state: { from: "movie", movieId: id } })}
                >
                  {d.comment}
                </p>
                <button
                  className="btn btn-danger"
                  style={{ padding: "4px 10px", fontSize: 12 }}
                  onClick={(e) => { e.stopPropagation(); setConfirmDeleteDiscussion(d.id); }}
                >
                  Delete
                </button>
              </div>
              <div
                className="discussion-card-meta"
                onClick={() => navigate(`/discussions/${d.id}`, { state: { from: "movie", movieId: id } })}
              >
                <span>{d.memberName}</span>
                <span>{new Date(d.date).toLocaleDateString()}</span>
                <span>{d.responseCount ?? 0} responses</span>
              </div>
            </div>
          ))}
        </div>
      )}

      {confirmDeleteDiscussion && (
        <ConfirmModal
          message="Are you sure you want to delete this discussion?"
          onConfirm={doDeleteDiscussion}
          onCancel={() => setConfirmDeleteDiscussion(null)}
        />
      )}
    </div>
  );
}