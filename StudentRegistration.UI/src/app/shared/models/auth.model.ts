export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
}

export interface RegisterRequest {
  nombre: string;
  email: string;
  password: string;
}
