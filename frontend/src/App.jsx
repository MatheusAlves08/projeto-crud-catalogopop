import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import ProcedimentosList from './pages/ProcedimentosList/ProcedimentosList';
import ProcedimentoForm from './pages/ProcedimentoForm/ProcedimentoForm';
import Login from './pages/Login/Login';
import Layout from './components/Layout/Layout';
import { authService } from './services/api';

const ProtectedRoute = ({ children }) => {
  if (!authService.isAuthenticated()) {
    return <Navigate to="/login" replace />;
  }
  return children;
};

const Dashboard = () => (
  <Layout title="Dashboard">
    <div className="card" style={{ padding: '2rem', textAlign: 'center' }}>
      <h2>Bem-vindo ao HGTX™ CatálogoPOP</h2>
      <p style={{ color: 'var(--text-muted)', marginTop: '1rem' }}>
        Selecione "Procedimentos" no menu lateral para gerenciar seus POPs.
      </p>
    </div>
  </Layout>
);

const App = () => {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        
        <Route path="/" element={
          <ProtectedRoute>
            <Dashboard />
          </ProtectedRoute>
        } />
        <Route path="/procedimentos" element={
          <ProtectedRoute>
            <ProcedimentosList />
          </ProtectedRoute>
        } />
        <Route path="/procedimentos/novo" element={
          <ProtectedRoute>
            <ProcedimentoForm />
          </ProtectedRoute>
        } />
        <Route path="/procedimentos/editar/:id" element={
          <ProtectedRoute>
            <ProcedimentoForm />
          </ProtectedRoute>
        } />
        
        {/* Redireciona rotas não encontradas */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  );
};

export default App;
