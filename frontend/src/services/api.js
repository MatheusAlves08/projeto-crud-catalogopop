import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5148/api', // Porta configurada no launchSettings.json do .NET
  headers: {
    'Content-Type': 'application/json'
  }
});

// Interceptor para adicionar o token JWT de forma automatizada
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Interceptor para tratamento global de erros
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Se o token expirar ou for inválido, podemos limpar e redirecionar
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('username');
    }
    const message = error.response?.data?.error || 'Erro inesperado no servidor.';
    return Promise.reject(new Error(message));
  }
);

export const authService = {
  login: async (username, password) => {
    const response = await api.post('/auth/login', { username, password });
    if (response.data?.token) {
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('username', response.data.username);
    }
    return response.data;
  },
  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
  },
  getCurrentUser: () => {
    return localStorage.getItem('username');
  },
  isAuthenticated: () => {
    return !!localStorage.getItem('token');
  }
};

export const popService = {
  getAll: () => api.get('/pops'),
  create: (data) => api.post('/pops', data),
  getById: (id) => api.get(`/pops/${id}`),
  update: (id, data) => api.put(`/pops/${id}`, data),
  delete: (id) => api.delete(`/pops/${id}`),
  checkCodigo: (codigo) => api.get(`/pops/check-codigo/${codigo}`)
};

export default api;
