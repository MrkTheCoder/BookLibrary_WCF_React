import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Row, Col } from "react-bootstrap";
import Bookcard from "../components/Bookcard";
import { listBooks } from "../actions/bookActions";

function HomeScreen() {
  const dispatch = useDispatch();
  const bookList = useSelector((state) => state.bookList);
  const { error, loading, books } = bookList;

  useEffect(() => {
    dispatch(listBooks());
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
