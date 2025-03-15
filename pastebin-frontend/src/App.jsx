import { BrowserRouter as Router, Routes, Route } from "react-router-dom"
import { Toaster } from "react-hot-toast"
import Navbar from "./components/Navbar"
import HomePage from "./pages/HomePage"
import ViewPastePage from "./pages/ViewPastePage"
import NotFoundPage from "./pages/NotFoundPage"
import ExpiredPastePage from "./pages/ExpiredPastePage"

function App() {
  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        <Navbar />
        <main className="container mx-auto px-4 py-8">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/paste/:pasteKey" element={<ViewPastePage />} />
            <Route path="/expired-paste" element={<ExpiredPastePage />} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </main>
        <Toaster position="bottom-right" />
      </div>
    </Router>
  )
}

export default App

