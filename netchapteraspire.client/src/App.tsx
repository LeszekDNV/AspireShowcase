import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Layout from './components/Layout'
import Home from './pages/Home'
import BlobStorage from './pages/BlobStorage'
import ServiceBus from './pages/ServiceBus'
import Database from './pages/Database'
import Mailing from './pages/Mailing'
import './App.css'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="blob-storage" element={<BlobStorage />} />
          <Route path="service-bus" element={<ServiceBus />} />
          <Route path="database" element={<Database />} />
          <Route path="mailing" element={<Mailing />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App
