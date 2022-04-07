import logo from "./logo.svg";
import { Container } from "react-bootstrap";
import Footer from "./components/Footer";
import HomeScreen from "./screens/HomeScreen";
import Header from "./components/Header";
import BookDetails from "./screens/BookDetails";
import Borrowers from "./screens/Borrowers";
import { HashRouter as Router, Route, Routes } from "react-router-dom";

function App() {
  return (
    <Router>
      <Header />
      <main className="py-3 mainScreen">
        <Container>
          <Routes>
            <Route path="/" element={<HomeScreen />} exact />
            <Route path="/book/:Isbn/" element={<BookDetails />} />
            <Route path="/admin/borrowers" element={<Borrowers />} />
          </Routes>
        </Container>
      </main>
      <Footer />
    </Router>
  );
}

export default App;
