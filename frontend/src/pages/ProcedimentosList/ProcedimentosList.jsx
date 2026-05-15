import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Plus, Search, Filter, Edit2, Trash2, ChevronLeft, ChevronRight, MoreHorizontal } from 'lucide-react';
import { motion } from 'framer-motion';
import { popService } from '../../services/api';
import Layout from '../../components/Layout/Layout';

const ProcedimentosList = () => {
  const navigate = useNavigate();
  const [pops, setPops] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchPops();
  }, []);

  const fetchPops = async () => {
    try {
      setLoading(true);
      const response = await popService.getAll();
      setPops(response.data);
      setError(null);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const getStatusStyle = (status) => {
    switch (status) {
      case 'Aprovado': return 'status-aprovado';
      case 'EmRevisao': return 'status-revisao';
      case 'Rascunho': return 'status-rascunho';
      case 'Obsoleto': return 'status-obsoleto';
      default: return '';
    }
  };

  const getStatusLabel = (status) => {
    switch (status) {
      case 'EmRevisao': return 'Em Revisão';
      default: return status;
    }
  };

  const getDepartamentoLabel = (dept) => {
  const map = { 0: 'Qualidade', 1: 'Produção', 2: 'Logística', 3: 'Administrativo' };
  return map[dept] ?? dept;
};

  const handleDelete = async (id, codigo) => {
    if (window.confirm(`Tem certeza que deseja excluir o procedimento ${codigo}?`)) {
      try {
        await popService.delete(id);
        fetchPops();
      } catch (err) {
        alert("Erro ao excluir: " + err.message);
      }
    }
  };

  return (
    <Layout title="Catálogo de Procedimentos Operacionais Padrão">
      <div className="list-actions">
        <div className="search-filters">
          <div className="input-with-icon search-main">
            <Search size={18} />
            <input type="text" placeholder="Pesquisar por código, título ou responsável..." />
          </div>
          
          <select className="filter-select">
            <option value="">Departamento</option>
            <option value="Qualidade">Qualidade</option>
            <option value="Producao">Produção</option>
            <option value="Logistica">Logística</option>
            <option value="Administrativo">Administrativo</option>
          </select>

          <select className="filter-select">
            <option value="">Status</option>
            <option value="Aprovado">Aprovado</option>
            <option value="EmRevisao">Em Revisão</option>
            <option value="Rascunho">Rascunho</option>
          </select>
        </div>

        <button className="btn-primary" onClick={() => navigate('/procedimentos/novo')}>
          <Plus size={20} />
          Novo POP
        </button>
      </div>

      <div className="table-container card">
        {loading ? (
          <div className="loading-state">Carregando procedimentos...</div>
        ) : error ? (
          <div className="error-state">{error}</div>
        ) : (
          <table className="pops-table">
            <thead>
              <tr>
                <th>Código POP</th>
                <th>Título</th>
                <th>Departamento</th>
                <th>Versão</th>
                <th>Status</th>
                <th>Responsável</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {pops.map((pop, index) => (
                <motion.tr 
                  key={pop.id}
                  initial={{ opacity: 0, x: -20 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ delay: index * 0.05 }}
                >
                  <td className="font-bold">{pop.codigo}</td>
                  <td>{pop.titulo}</td>
                  <td>{pop.departamento}</td>
                  <td className="version-cell">v{pop.versao}</td>
                  <td>
                    <span className={`status-badge ${getStatusStyle(pop.status)}`}>
                      {getStatusLabel(pop.status)}
                    </span>
                  </td>
                  <td>{pop.responsavel}</td>
                  <td>
                    <div className="action-btns">
                      <button 
                        className="action-btn edit" 
                        title="Editar"
                        onClick={() => navigate(`/procedimentos/editar/${pop.id}`)}
                      >
                        <Edit2 size={16} />
                      </button>
                      <button 
                        className="action-btn delete" 
                        title="Excluir"
                        onClick={() => handleDelete(pop.id, pop.codigo)}
                      >
                        <Trash2 size={16} />
                      </button>
                    </div>
                  </td>
                </motion.tr>
              ))}
              {pops.length === 0 && (
                <tr>
                  <td colSpan="7" className="empty-row">Nenhum procedimento encontrado.</td>
                </tr>
              )}
            </tbody>
          </table>
        )}

        <div className="table-footer">
          <span className="results-count">Mostrando 1 a {pops.length} de {pops.length} resultados</span>
          <div className="pagination">
            <button className="page-btn disabled"><ChevronLeft size={18} /></button>
            <button className="page-btn active">1</button>
            <button className="page-btn"><ChevronRight size={18} /></button>
          </div>
          <div className="rows-per-page">
            <select>
              <option>10 por página</option>
              <option>20 por página</option>
            </select>
          </div>
        </div>
      </div>

      <style dangerouslySetInnerHTML={{ __html: `
        .list-actions {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-bottom: 1.5rem;
          gap: 1rem;
          flex-wrap: wrap;
        }

        .search-filters {
          display: flex;
          gap: 1rem;
          flex: 1;
          min-width: 300px;
        }

        .input-with-icon {
          position: relative;
          display: flex;
          align-items: center;
          background: white;
          border: 1px solid var(--border);
          border-radius: var(--radius-md);
          padding: 0 1rem;
          transition: var(--transition);
        }

        .input-with-icon:focus-within {
          border-color: var(--primary);
          box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
        }

        .input-with-icon svg {
          color: var(--text-muted);
          margin-right: 10px;
        }

        .input-with-icon input {
          border: none;
          outline: none;
          height: 42px;
          width: 100%;
          font-size: 0.95rem;
        }

        .search-main {
          flex: 1;
        }

        .filter-select {
          background: white;
          border: 1px solid var(--border);
          border-radius: var(--radius-md);
          padding: 0 1rem;
          height: 44px;
          min-width: 160px;
          color: var(--text-main);
          font-family: inherit;
          font-size: 0.95rem;
          outline: none;
          cursor: pointer;
          transition: var(--transition);
        }

        .filter-select:focus {
          border-color: var(--primary);
        }

        .btn-primary {
          background: var(--primary);
          color: white;
          padding: 0 1.5rem;
          height: 44px;
          border-radius: var(--radius-md);
          display: flex;
          align-items: center;
          gap: 8px;
          font-weight: 600;
          box-shadow: 0 4px 12px rgba(37, 99, 235, 0.2);
        }

        .btn-primary:hover {
          background: var(--primary-hover);
          transform: translateY(-1px);
        }

        .card {
          background: var(--bg-card);
          border-radius: var(--radius-lg);
          border: 1px solid var(--border);
          box-shadow: var(--shadow-md);
          overflow: hidden;
        }

        .pops-table {
          width: 100%;
          border-collapse: collapse;
          text-align: left;
        }

        .pops-table th {
          background: #f8fafc;
          padding: 1rem 1.5rem;
          font-weight: 600;
          font-size: 0.85rem;
          color: var(--text-muted);
          text-transform: uppercase;
          letter-spacing: 0.5px;
          border-bottom: 1px solid var(--border);
        }

        .pops-table td {
          padding: 1.25rem 1.5rem;
          border-bottom: 1px solid var(--border);
          font-size: 0.95rem;
          color: var(--text-main);
        }

        .pops-table tr:last-child td {
          border-bottom: none;
        }

        .pops-table tr:hover td {
          background: #f1f5f9;
        }

        .font-bold {
          font-weight: 600;
        }

        .version-cell {
          color: var(--secondary);
          font-weight: 500;
        }

        .status-badge {
          display: inline-flex;
          align-items: center;
          padding: 0.25rem 0.75rem;
          border-radius: 20px;
          font-size: 0.85rem;
          font-weight: 600;
        }

        .status-aprovado { background: #dcfce7; color: #166534; border: 1px solid #bbf7d0; }
        .status-revisao { background: #fef9c3; color: #854d0e; border: 1px solid #fef08a; }
        .status-rascunho { background: #f1f5f9; color: #475569; border: 1px solid #e2e8f0; }
        .status-obsoleto { background: #fee2e2; color: #991b1b; border: 1px solid #fecaca; }

        .action-btns {
          display: flex;
          gap: 8px;
        }

        .action-btn {
          width: 34px;
          height: 34px;
          border-radius: 8px;
          display: flex;
          align-items: center;
          justify-content: center;
          background: white;
          border: 1px solid var(--border);
          color: var(--text-muted);
          transition: var(--transition);
        }

        .action-btn.edit:hover {
          color: var(--primary);
          border-color: var(--primary);
          background: rgba(37, 99, 235, 0.05);
        }

        .action-btn.delete:hover {
          color: var(--danger);
          border-color: var(--danger);
          background: rgba(239, 68, 68, 0.05);
        }

        .table-footer {
          padding: 1.5rem;
          display: flex;
          align-items: center;
          justify-content: space-between;
          border-top: 1px solid var(--border);
        }

        .results-count {
          font-size: 0.9rem;
          color: var(--text-muted);
        }

        .pagination {
          display: flex;
          gap: 4px;
        }

        .page-btn {
          width: 36px;
          height: 36px;
          display: flex;
          align-items: center;
          justify-content: center;
          border-radius: 8px;
          border: 1px solid var(--border);
          background: white;
          font-weight: 500;
          color: var(--text-main);
        }

        .page-btn.active {
          background: var(--primary);
          color: white;
          border-color: var(--primary);
        }

        .page-btn:hover:not(.active):not(.disabled) {
          background: #f1f5f9;
        }

        .page-btn.disabled {
          opacity: 0.5;
          cursor: not-allowed;
        }

        .rows-per-page select {
          border: 1px solid var(--border);
          border-radius: 8px;
          padding: 0.25rem 0.5rem;
          font-family: inherit;
          font-size: 0.9rem;
          outline: none;
        }

        .loading-state, .error-state, .empty-row {
          padding: 4rem;
          text-align: center;
          color: var(--text-muted);
          font-weight: 500;
        }

        .error-state {
          color: var(--danger);
        }
      `}} />
    </Layout>
  );
};

export default ProcedimentosList;
