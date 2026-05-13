import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import ProcedimentosList from './pages/ProcedimentosList/ProcedimentosList';
import ProcedimentoForm from './pages/ProcedimentoForm/ProcedimentoForm';
import Layout from './components/Layout/Layout';

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
        <Route path="/" element={<Dashboard />} />
        <Route path="/procedimentos" element={<ProcedimentosList />} />
        <Route path="/procedimentos/novo" element={<ProcedimentoForm />} />
        <Route path="/procedimentos/editar/:id" element={<ProcedimentoForm />} />
        
        {/* Redireciona rotas não encontradas para o dashboard */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  );
};

export default App;
