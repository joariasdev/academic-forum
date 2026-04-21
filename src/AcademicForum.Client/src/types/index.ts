export interface ApiResponse<T> {
  data: T;
  message: string | null;
  success: boolean;
  statusCode: number;
}

export interface Member {
  id: number;
  name: string;
  email: string;
  role: string;
  discussions: Discussion[];
  responses: Response[];
  attendeeRecords: AttendeeRecord[];
}

export interface Venue {
  id: number;
  name: string;
  address: string;
  capacity: number;
  events: Event[];
}

export interface Movie {
  id: number;
  title: string;
  sinopsis: string;
  director: string;
  genre: string;
  releaseDate: string;
  events: Event[];
  discussions: Discussion[];
}

export interface Event {
  id: number;
  title: string;
  description: string;
  date: string;
  movieId: number;
  movieTitle: string;
  venueId: number;
  venueName: string;
  discussions: Discussion[];
  attendeeRecords: AttendeeRecord[];
}

export interface Discussion {
  id: number;
  comment: string;
  date: string;
  movieId: number;
  eventId: number;
  memberId: number;
  memberName: string;
  responseCount: number;
  responses: Response[];
}

export interface Response {
  id: number;
  comment: string;
  date: string;
  discussionId: number;
  memberId: number;
  memberName: string;
}

export interface AttendeeRecord {
  id: number;
  hasAttended: boolean;
  eventId: number;
  memberId: number;
  memberName: string;
}