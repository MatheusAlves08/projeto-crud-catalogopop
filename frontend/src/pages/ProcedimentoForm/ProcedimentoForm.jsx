import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { 
  ArrowLeft, 
  Save, 
  X, 
  AlertCircle, 
  CheckCircle2, 
  Info,
  History,
  Trash2,
  Calendar
} from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { popService } from '../../services/api';
import Layout from '../../components/Layout/Layout';

const ProcedimentoForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const isEdit = Boolean(id);

  const [formData, setFormData] = useState({
    codigo: '',
    titulo: '',
    descricao: '',
    departamento: '',
    responsavel: '',
    status: 'Rascunho',
    versao: '1.0',
    dataRevisao: ''
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);
  const [codeCheck, setCodeCheck] = useState({ loading: false, exists: false, checked: false });

  // Simulação de histórico para a tela de edição
  const versionHistory = [
    { version: 'v1.0', date: '01/01/2024' },
    { version: 'v2.0', date: '15/06/2024' },
    { version: 'v2.1', date: '10/01/2025', current: true },
  ];

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    
    if (name === 'codigo') {
      setCodeCheck({ loading: false, exists: false, checked: false });
    }
  };

  const handleBlurCodigo = () => {
    if (!formData.codigo) return;
    
    // Simulação de checagem de código (backend precisa de endpoint pra isso)
    setCodeCheck({ loading: true, exists: false, checked: false });
    setTimeout(() => {
      // Se o código for "POP-003", simulamos que já existe (como na imagem)
      const exists = formData.codigo === 'POP-003';
      setCodeCheck({ loading: false, exists, checked: true });
    }, 800);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      // Converte o enum de departamento para número (como o backend espera)
      const deptMap = { 'Qualidade': 0, 'Producao': 1, 'Logistica': 2, 'Administrativo': 3 };
      const dataToSave = {
        ...formData,
        departamento: deptMap[formData.departamento] || 0
      };

      await popService.create(dataToSave);
      setSuccess(true);
      setTimeout(() => navigate('/procedimentos'), 2000);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Layout title={isEdit ? `Editar Procedimento: ${formData.codigo || '...'}` : 'Novo Procedimento Operacional Padrão'}>
      <div className="form-header">
        <button className="btn-ghost" onClick={() => navigate('/procedimentos')}>
          <ArrowLeft size={20} />
          Voltar
        </button>
        {!isEdit && <p className="subtitle">Preencha as informações para cadastrar um novo POP no sistema.</p>}
      </div>

      <AnimatePresence>
        {success && (
          <motion.div 
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0 }}
            className="alert alert-success"
          >
            <CheckCircle2 size={20} />
            Procedimento salvo com sucesso! Redirecionando...
          </motion.div>
        )}

        {error && (
          <motion.div 
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            className="alert alert-danger"
          >
            <AlertCircle size={20} />
            {error}
          </motion.div>
        )}
      </AnimatePresence>

      {isEdit && (
        <div className="info-banner info">
          <Info size={20} />
          <p>Este procedimento está com status <strong>Aprovado</strong>. Ao salvar as alterações, a versão será incrementada automaticamente de <strong>v2.1 para v2.2</strong>.</p>
        </div>
      )}

      <form className="pop-form card" onSubmit={handleSubmit}>
        <div className="form-grid">
          {/* Lado Esquerdo */}
          <div className="form-column">
            <div className="form-group">
              <label>Código POP *</label>
              <div className="input-container">
                <input 
                  type="text" 
                  name="codigo"
                  value={formData.codigo}
                  onChange={handleChange}
                  onBlur={handleBlurCodigo}
                  placeholder="Ex: POP-001"
                  className={codeCheck.exists ? 'error' : codeCheck.checked && !codeCheck.exists ? 'success' : ''}
                  required 
                />
              </div>
              
              {codeCheck.loading && <p className="input-hint">Verificando disponibilidade...</p>}
              
              {codeCheck.checked && !codeCheck.exists && (
                <div className="feedback-message success">
                  <CheckCircle2 size={16} />
                  <span>Código disponível! Nenhum procedimento encontrado com este código.</span>
                </div>
              )}

              {codeCheck.exists && (
                <div className="feedback-message error">
                  <AlertCircle size={16} />
                  <span>Código já cadastrado! Um procedimento com este código já existe no sistema.</span>
                </div>
              )}

              {codeCheck.exists && (
                <div className="existing-pop-card card">
                  <Files size={24} className="icon" />
                  <div className="info">
                    <strong>{formData.codigo} - Controle de Estoque</strong>
                    <span>Departamento: Logística | Status: Aprovado | Versão: v3.0</span>
                    <a href="#">Ver procedimento existente ↗</a>
                  </div>
                </div>
              )}
            </div>

            <div className="form-group">
              <label>Título do Procedimento *</label>
              <input 
                type="text" 
                name="titulo"
                value={formData.titulo}
                onChange={handleChange}
                placeholder="Ex: Recebimento de Materiais" 
                required 
              />
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>Departamento *</label>
                <select name="departamento" value={formData.departamento} onChange={handleChange} required>
                  <option value="">Selecione...</option>
                  <option value="Qualidade">Qualidade</option>
                  <option value="Producao">Produção</option>
                  <option value="Logistica">Logística</option>
                  <option value="Administrativo">Administrativo</option>
                </select>
              </div>
              <div className="form-group">
                <label>Responsável *</label>
                <input 
                  type="text" 
                  name="responsavel"
                  value={formData.responsavel}
                  onChange={handleChange}
                  placeholder="Nome do responsável" 
                  required 
                />
              </div>
            </div>
          </div>

          {/* Lado Direito */}
          <div className="form-column">
            <div className="form-group">
              <label>Descrição Detalhada *</label>
              <textarea 
                name="descricao"
                value={formData.descricao}
                onChange={handleChange}
                rows="5"
                placeholder="Descreva detalhadamente o objetivo, o escopo, as responsabilidades..."
                required
              ></textarea>
              <div className="char-count">{formData.descricao.length}/1000</div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>Versão</label>
                <div className="version-input-group">
                  <input type="text" value={formData.versao} readOnly className="readonly" />
                  {isEdit && <span className="version-auto-badge">Automático</span>}
                </div>
              </div>
              <div className="form-group">
                <label>Status *</label>
                <select name="status" value={formData.status} onChange={handleChange} required>
                  <option value="Rascunho">Rascunho</option>
                  <option value="EmRevisao">Em Revisão</option>
                  <option value="Aprovado">Aprovado</option>
                  <option value="Obsoleto">Obsoleto</option>
                </select>
              </div>
            </div>

            <div className="form-group">
              <label>Data de Última Revisão</label>
              <div className="input-with-icon">
                <Calendar size={18} />
                <input 
                  type="date" 
                  name="dataRevisao"
                  value={formData.dataRevisao}
                  onChange={handleChange} 
                />
              </div>
            </div>
          </div>
        </div>

        <div className="form-actions">
          {isEdit && (
            <button type="button" className="btn-outline-danger">
              <Trash2 size={18} />
              Excluir Procedimento
            </button>
          )}
          <div className="right-actions">
            <button type="button" className="btn-ghost" onClick={() => navigate('/procedimentos')}>Cancelar</button>
            {isEdit && <button type="button" className="btn-outline">Visualizar</button>}
            <button type="submit" className="btn-primary" disabled={loading || codeCheck.exists}>
              <Save size={18} />
              {loading ? 'Salvando...' : isEdit ? 'Salvar Alterações' : 'Salvar Procedimento'}
            </button>
          </div>
        </div>
      </form>

      {isEdit && (
        <div className="version-history-section animate-fade-in" style={{ marginTop: '2rem' }}>
          <h3>Histórico de Versões</h3>
          <div className="timeline card">
            <div className="timeline-track"></div>
            {versionHistory.map((item, index) => (
              <div key={item.version} className={`timeline-item ${item.current ? 'current' : ''}`}>
                <div className="timeline-dot"></div>
                <div className="timeline-content">
                  <span className="version">{item.version}</span>
                  <span className="date">{item.date}</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      <style dangerouslySetInnerHTML={{ __html: `
        .form-header {
          margin-bottom: 2rem;
        }

        .subtitle {
          color: var(--text-muted);
          margin-top: 0.5rem;
          font-size: 0.95rem;
        }

        .pop-form {
          padding: 2rem;
        }

        .form-grid {
          display: grid;
          grid-template-columns: 1fr 1fr;
          gap: 3rem;
        }

        @media (max-width: 1024px) {
          .form-grid { grid-template-columns: 1fr; gap: 1.5rem; }
        }

        .form-group {
          margin-bottom: 1.5rem;
          display: flex;
          flex-direction: column;
          gap: 6px;
        }

        .form-group label {
          font-size: 0.9rem;
          font-weight: 600;
          color: var(--text-main);
        }

        .form-row {
          display: grid;
          grid-template-columns: 1fr 1fr;
          gap: 1rem;
        }

        input, select, textarea {
          width: 100%;
          padding: 0.75rem 1rem;
          border: 1px solid var(--border);
          border-radius: var(--radius-md);
          font-family: inherit;
          font-size: 1rem;
          transition: var(--transition);
          background: #fcfcfc;
        }

        input:focus, select:focus, textarea:focus {
          border-color: var(--primary);
          background: white;
          box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
          outline: none;
        }

        input.success { border-color: var(--success); }
        input.error { border-color: var(--danger); background: #fff1f2; }

        .readonly {
          background: #f1f5f9;
          cursor: not-allowed;
        }

        .version-input-group {
          position: relative;
          display: flex;
          align-items: center;
        }

        .version-auto-badge {
          position: absolute;
          right: 12px;
          background: var(--primary);
          color: white;
          font-size: 0.7rem;
          font-weight: 700;
          padding: 2px 8px;
          border-radius: 20px;
          text-transform: uppercase;
        }

        .char-count {
          font-size: 0.75rem;
          color: var(--text-muted);
          text-align: right;
        }

        .feedback-message {
          display: flex;
          align-items: center;
          gap: 6px;
          font-size: 0.85rem;
          font-weight: 500;
          margin-top: 4px;
        }

        .feedback-message.success { color: var(--success); background: #f0fdf4; padding: 0.75rem; border-radius: 8px; }
        .feedback-message.error { color: var(--danger); background: #fef2f2; padding: 0.75rem; border-radius: 8px; }

        .existing-pop-card {
          margin-top: 1rem;
          padding: 1rem;
          display: flex;
          gap: 1rem;
          align-items: center;
          background: #f8fafc;
        }

        .existing-pop-card .icon { color: var(--primary); }
        .existing-pop-card .info { display: flex; flex-direction: column; font-size: 0.85rem; }
        .existing-pop-card .info a { color: var(--primary); font-weight: 600; margin-top: 4px; }

        .info-banner {
          display: flex;
          align-items: center;
          gap: 12px;
          padding: 1rem 1.5rem;
          border-radius: var(--radius-md);
          margin-bottom: 1.5rem;
          border: 1px solid;
        }

        .info-banner.info { background: #eff6ff; color: #1e40af; border-color: #bfdbfe; }

        .alert {
          padding: 1rem 1.5rem;
          border-radius: var(--radius-md);
          margin-bottom: 1.5rem;
          display: flex;
          align-items: center;
          gap: 12px;
          font-weight: 500;
        }

        .alert-success { background: #dcfce7; color: #166534; }
        .alert-danger { background: #fee2e2; color: #991b1b; }

        .form-actions {
          margin-top: 2rem;
          padding-top: 2rem;
          border-top: 1px solid var(--border);
          display: flex;
          justify-content: space-between;
          align-items: center;
        }

        .right-actions {
          display: flex;
          gap: 1rem;
        }

        .btn-outline-danger {
          background: white;
          color: var(--danger);
          border: 1px solid #fecaca;
          padding: 0.75rem 1.25rem;
          border-radius: var(--radius-md);
          display: flex;
          align-items: center;
          gap: 8px;
          font-weight: 600;
        }

        .btn-outline-danger:hover { background: #fef2f2; }

        .btn-ghost {
          background: transparent;
          color: var(--text-muted);
          padding: 0.75rem 1.25rem;
          border-radius: var(--radius-md);
          display: flex;
          align-items: center;
          gap: 8px;
          font-weight: 600;
        }

        .btn-ghost:hover { background: rgba(0, 0, 0, 0.05); color: var(--text-main); }

        .btn-outline {
          background: white;
          color: var(--text-main);
          border: 1px solid var(--border);
          padding: 0.75rem 1.5rem;
          border-radius: var(--radius-md);
          font-weight: 600;
        }

        /* Timeline Styles */
        .timeline {
          padding: 3rem 2rem;
          display: flex;
          justify-content: space-between;
          position: relative;
        }

        .timeline-track {
          position: absolute;
          top: 50%;
          left: 4rem;
          right: 4rem;
          height: 2px;
          background: var(--border);
          transform: translateY(-50%);
          z-index: 1;
        }

        .timeline-item {
          position: relative;
          z-index: 2;
          display: flex;
          flex-direction: column;
          align-items: center;
          gap: 12px;
          flex: 1;
        }

        .timeline-dot {
          width: 14px;
          height: 14px;
          border-radius: 50%;
          background: white;
          border: 2px solid var(--border);
          transition: var(--transition);
        }

        .timeline-item.current .timeline-dot {
          background: var(--primary);
          border-color: var(--primary);
          box-shadow: 0 0 0 4px rgba(37, 99, 235, 0.2);
          transform: scale(1.3);
        }

        .timeline-content {
          text-align: center;
          display: flex;
          flex-direction: column;
        }

        .timeline-content .version { font-weight: 700; font-size: 0.95rem; }
        .timeline-content .date { font-size: 0.8rem; color: var(--text-muted); }
      `}} />
    </Layout>
  );
};

export default ProcedimentoForm;
