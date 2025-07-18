import { useState } from 'react'
import LoginForm from './LoginForm.tsx'
import Dashboard from './Dashboard.tsx'
import './App.css'
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import TasksPage from './TasksPage.tsx';
import Register from './Register.tsx';


function App() {
  const [count, setCount] = useState(0)

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LoginForm />} />
        <Route path="/home" element={<Dashboard />} />
        <Route path='/tasks' element={<TasksPage/>} />
        <Route path='/register' element={<Register/>} />

      </Routes>
    </BrowserRouter>
  )
}

export default App
