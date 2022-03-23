import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { Row, Col } from "react-bootstrap";
import Bookcard from "../components/Bookcard";
import { listBooks } from "../actions/bookActions";
import Loader from "../components/Loader";
import Message from "../components/Message";
import Paginate from "../components/Paginate";

function HomeScreen() {
  const dispatch = useDispatch();
  const bookList = useSelector((state) => state.bookList);
  const { error, loading, books } = bookList;
  const [currentPage, setCurrentPage] = useState(1);
  const [searchParams, setSearchParams] = useSearchParams();

  useEffect(() => {
    if (searchParams.get("page")) {
      setCurrentPage(Number(searchParams.get("page")));

      dispatch(listBooks(searchParams.get("page")));
    } else {
      dispatch(listBooks());
    }
  }, [dispatch, searchParams]);

  return (
    <div>
      {loading ? (
        <Loader />
      ) : error ? (
        <Message variant="danger">{error}</Message>
      ) : (
        <div className="cardRows">
          <Row data-testid="cardRow" className="mainScreen">
            {books.map((book) => (
              <Col key={book.Id} sm={12} md={6} lg={4} xl={3}>
                <Bookcard book={book} />
              </Col>
            ))}
          </Row>
          <Paginate page={currentPage} pages={3} />
        </div>
      )}
    </div>
  );
}

export default HomeScreen;
