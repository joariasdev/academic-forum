import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { moviesApi } from "../api/services";
import type { Movie, ApiResponse } from "../types";
import ConfirmModal from "../components/ConfirmModal";

const emptyForm = {
  title: "",
  sinopsis: "",
  director: "",
  genre: "",
  releaseDate: "",
};

export default function MoviesPage() {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [editingMovie, setEditingMovie] = useState<Movie | null>(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState<Movie | null>(null);
  const navigate = useNavigate();

  useEffect(() => { fetchMovies(); }, []);

  const fetchMovies = () => {
    setLoading(true);
    moviesApi
      .getAll()
      .then((res) => {
        const body: ApiResponse<Movie[]> = res.data;
        setMovies(body.data);
      })
      .catch(() => setError("Failed to load movies."))
      .finally(() => setLoading(false));
  };

  const openCreate = () => {
    setEditingMovie(null);
    setForm(emptyForm);
    setShowModal(true);
  };

  const openEdit = (movie: Movie, e: React.MouseEvent) => {
    e.stopPropagation();
    setEditingMovie(movie);
    setForm({
      title: movie.title,
      sinopsis: movie.sinopsis,
      director: movie.director,
      genre: movie.genre,
      releaseDate: movie.releaseDate.split("T")[0],
    });
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingMovie(null);
    setForm(emptyForm);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = () => {
    setSaving(true);
    const request = editingMovie
      ? moviesApi.update(String(editingMovie.id), form)
      : moviesApi.create(form);
    request
      .then(() => { closeModal(); fetchMovies(); })
      .catch(() => setError("Failed to save movie."))
      .finally(() => setSaving(false));
  };

  const confirmDeleteMovie = () => {
    if (!confirmDelete) return;
    moviesApi
      .delete(String(confirmDelete.id))
      .then(() => { setConfirmDelete(null); fetchMovies(); })
      .catch(() => setError("Failed to delete movie."));
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h2>Movies</h2>
          <p>Browse and manage all movies in the forum.</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>+ New Movie</button>
      </div>

      <div className="table-container">
        <div className="table-toolbar">
          <span className="table-toolbar-title">All Movies</span>
        </div>
        {movies.length === 0 ? (
          <div className="empty-state">No movies found.</div>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Title</th>
                <th>Director</th>
                <th>Genre</th>
                <th>Release Date</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {movies.map((movie) => (
                <tr
                  key={movie.id}
                  onClick={() => navigate(`/movies/${movie.id}`)}
                  style={{ cursor: "pointer" }}
                >
                  <td>{movie.title}</td>
                  <td>{movie.director}</td>
                  <td>{movie.genre}</td>
                  <td>{new Date(movie.releaseDate).toLocaleDateString()}</td>
                  <td onClick={(e) => e.stopPropagation()}>
                    <div style={{ display: "flex", gap: 8 }}>
                      <button className="btn btn-ghost" onClick={(e) => openEdit(movie, e)}>Edit</button>
                      <button className="btn btn-danger" onClick={() => setConfirmDelete(movie)}>Delete</button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h3 className="modal-title">{editingMovie ? "Edit Movie" : "New Movie"}</h3>

            <div className="form-group">
              <label className="form-label">Title</label>
              <input className="form-input" name="title" value={form.title} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Director</label>
              <input className="form-input" name="director" value={form.director} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Genre</label>
              <input className="form-input" name="genre" value={form.genre} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Release Date</label>
              <input className="form-input" name="releaseDate" type="date" value={form.releaseDate} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Synopsis</label>
              <textarea className="form-textarea" name="sinopsis" value={form.sinopsis} onChange={handleChange} />
            </div>

            <div className="modal-actions">
              <button className="btn btn-ghost" onClick={closeModal}>Cancel</button>
              <button className="btn btn-primary" onClick={handleSubmit} disabled={saving}>
                {saving ? "Saving..." : "Save"}
              </button>
            </div>
          </div>
        </div>
      )}

      {confirmDelete && (
        <ConfirmModal
          message={`Are you sure you want to delete "${confirmDelete.title}"?`}
          onConfirm={confirmDeleteMovie}
          onCancel={() => setConfirmDelete(null)}
        />
      )}
    </div>
  );
}