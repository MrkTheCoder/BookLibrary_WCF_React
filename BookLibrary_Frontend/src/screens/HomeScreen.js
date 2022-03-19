import React, { useEffect, useState } from "react";
import { Row, Col } from "react-bootstrap";
import Bookcard from "../components/Bookcard";

import axios from "axios";

function HomeScreen() {
  const [books, setBooks] = useState([]);

  useEffect(() => {
    async function fetchBooks() {
      const response = await axios.get("http://localhost:51202/LibraryBooks");

      setBooks(response.data);
    }

    fetchBooks();
  }, []);
  return (
    <div className="cardRows">
      <Row data-testid="cardRow" className="mainScreen">
        {books.map((book) => (
          <Col key={book.Id} sm={12} md={6} lg={4} xl={3}>
            <Bookcard book={book} />
          </Col>
        ))}
      </Row>
    </div>
  );
}

export default HomeScreen;
