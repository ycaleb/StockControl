import React, { useEffect, useState } from 'react';
import { FaPlus, FaEdit, FaTrash, FaSearch, FaFilePdf, FaUserAlt, FaArrowsAltH } from 'react-icons/fa';
import './App.css';

const App = () => {
  const [materiais, setMateriais] = useState([]);

  useEffect(() => {
    fetchMateriais();
  }, []);

  const fetchMateriais = async () => {
    const res = await fetch('https://localhost:5001/api/materiais');
    if (!res.ok) {
      alert('Erro ao buscar materiais');
      return;
    }
    const data = await res.json();
    setMateriais(data);
  };

  const handleAdd = () => alert('Adicionar material');
  const handleEdit = (id) => alert(`Editar material ${id}`);
  const handleView = (id) => alert(`Visualizar material ${id}`);
  const handleDelete = (id) => alert(`Excluir material ${id}`);
  const handleReport = () => alert('Gerar relatório');

  return (
    <div className="p-6">
      <div className="text-center">
        <div className="flex items-center gap-2 mb-1">
          <img
            src="/iconhome.ico"
            alt="StockControl Icon"
            className="w-45 h-15 object-contain"
          />
          <p className="text-xl font-bold leading-none t1">StockControl</p>
        </div>
      </div>
      <div className="flex gap-2 mb-4">
        <button onClick={handleAdd} className="bg-blue-500 text-white px-3 py-1 rounded flex items-center gap-1">
          <FaUserAlt /> Usuário
        </button>
        <button onClick={handleAdd} className="bg-green-500 text-white px-3 py-1 rounded flex items-center gap-1">
          <FaPlus /> Adicionar
        </button>
        <button onClick={handleAdd} className="bg-yellow-500 text-white px-3 py-1 rounded flex items-center gap-1">
          <FaArrowsAltH /> Movimentação
        </button>
        <button onClick={handleReport} className="bg-red-500 text-white px-3 py-1 rounded flex items-center gap-1">
          <FaFilePdf /> Relatório
        </button>
      </div>
      <table className="min-w-full border">
        <thead>
          <tr className="bg-gray-200">
            <th className="px-4 py-2 border">Código</th>
            <th className="px-4 py-2 border">Nome</th>
            <th className="px-4 py-2 border">Unidade</th>
            <th className="px-4 py-2 border">Custo</th>
            <th className="px-4 py-2 border">Em Estoque</th>
            <th className="px-4 py-2 border">Ações</th>
          </tr>
        </thead>
        <tbody>
          {materiais.map((m) => (
            <tr key={m.id} className="text-center">
              <td className="px-4 py-2 border">{m.id}</td>
              <td className="px-4 py-2 border">{m.nome}</td>
              <td className="px-4 py-2 border">{m.unidadeMedida}</td>
              <td className="px-4 py-2 border">R$ {m.custoUnitario.toFixed(2)}</td>
              <td className="px-4 py-2 border">{m.quantidadeEstoque}</td>
              <td className="px-4 py-2 border flex justify-center gap-2">
                <button onClick={() => handleView(m.id)} className="bg-gray-300 px-2 py-1 rounded"><FaSearch /></button>
                <button onClick={() => handleEdit(m.id)} className="bg-yellow-400 px-2 py-1 rounded"><FaEdit /></button>
                <button onClick={() => handleDelete(m.id)} className="bg-red-500 text-white px-2 py-1 rounded"><FaTrash /></button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default App;
