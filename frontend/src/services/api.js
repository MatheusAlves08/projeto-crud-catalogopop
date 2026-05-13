import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5148/api', // Porta configurada no launchSettings.json do .NET
  headers: {
    'Content-Type': 'application/json'
  }
});

// Interceptor para tratamento global de erros (opcional, mas bom para UX)
api.interceptors.response.use(
  (response) => response,
  (error) => {
    const message = error.response?.data?.error || 'Erro inesperado no servidor.';
    return Promise.reject(new Error(message));
  }
);

export const popService = {
  getAll: () => api.get('/pops'),
  create: (data) => api.post('/pops', data),
  // Futuros:
  // getById: (id) => api.get(`/pops/${id}`),
  // update: (id, data) => api.put(`/pops/${id}`, data),
  // delete: (id) => api.delete(`/pops/${id}`),
  // checkCodigo: (codigo) => api.get(`/pops/check-codigo/${codigo}`)
};

export default api;
