import { apiClient } from "./client";

export const eventsApi = {
  getAll: () => apiClient.get("/api/events"),
  getById: (id: string) => apiClient.get(`/api/events/${id}`),
  create: (data: unknown) => apiClient.post("/api/events", data),
  update: (id: string, data: unknown) => apiClient.put(`/api/events/${id}`, data),
  delete: (id: string) => apiClient.delete(`/api/events/${id}`),
};

export const discussionsApi = {
  getAll: () => apiClient.get("/api/discussions"),
  getById: (id: string) => apiClient.get(`/api/discussions/${id}`),
  create: (data: unknown) => apiClient.post("/api/discussions", data),
  update: (id: string, data: unknown) => apiClient.put(`/api/discussions/${id}`, data),
  delete: (id: string) => apiClient.delete(`/api/discussions/${id}`),
};

export const membersApi = {
  getAll: () => apiClient.get("/api/members"),
  getById: (id: string) => apiClient.get(`/api/members/${id}`),
  create: (data: unknown) => apiClient.post("/api/members", data),
  update: (id: string, data: unknown) => apiClient.put(`/api/members/${id}`, data),
  delete: (id: string) => apiClient.delete(`/api/members/${id}`),
};

export const venuesApi = {
  getAll: () => apiClient.get("/api/venues"),
  getById: (id: string) => apiClient.get(`/api/venues/${id}`),
  create: (data: unknown) => apiClient.post("/api/venues", data),
  update: (id: string, data: unknown) => apiClient.put(`/api/venues/${id}`, data),
  delete: (id: string) => apiClient.delete(`/api/venues/${id}`),
};

export const moviesApi = {
  getAll: () => apiClient.get("/api/movies"),
  getById: (id: string) => apiClient.get(`/api/movies/${id}`),
  create: (data: unknown) => apiClient.post("/api/movies", data),
  update: (id: string, data: unknown) => apiClient.put(`/api/movies/${id}`, data),
  delete: (id: string) => apiClient.delete(`/api/movies/${id}`),
};

export const responsesApi = {
  getAll: () => apiClient.get("/api/responses"),
  getByDiscussionId: (discussionId: string) => apiClient.get(`/api/responses?discussionId=${discussionId}`),
  create: (data: unknown) => apiClient.post("/api/responses", data),
  delete: (id: string) => apiClient.delete(`/api/responses/${id}`),
};

export const attendeeRecordsApi = {
  getAll: () => apiClient.get("/api/attendeerecords"),
  getByEventId: (eventId: string) => apiClient.get(`/api/attendeerecords?eventId=${eventId}`),
  create: (data: unknown) => apiClient.post("/api/attendeerecords", data),
  update: (id: string, data: unknown) => apiClient.put(`/api/attendeerecords/${id}`, data),
  delete: (id: string) => apiClient.delete(`/api/attendeerecords/${id}`),
};