import React from "react";
import { Card, Row, Col } from "react-bootstrap";
import Bookcard from "../components/Bookcard";
import books from "../data";

function HomeScreen() {
  return (
    <div className="cardRows">
      <Row className="mainScreen">
        {books.map((book) => (
          <Col key={book.id} sm={12} md={6} lg={4} xl={3}>
            <Bookcard book={book} />
          </Col>
        ))}
      </Row>
    </div>
  );
}

export default HomeScreen;
