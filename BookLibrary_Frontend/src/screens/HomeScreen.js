import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { Row, Col } from "react-bootstrap";
import Bookcard from "../components/Bookcard";
import { listBooks } from "../actions/bookActions";
import Loader from "../components/Loader";
import Message from "../components/Message";
import Paginate from "../components/Paginate";
import Navigation from "../components/Navigation";

function HomeScreen() {
  const dispatch = useDispatch();
  const bookList = useSelector((state) => state.bookList);
  const { error, loading, books, headers } = bookList;

  const [currentPage, setCurrentPage] = useState(1);
  const [currentItem, setCurrentItem] = useState(10);

  const [searchParams, setSearchParams] = useSearchParams();
  const filtersFromState = useSelector((state) => state.filters);

  const { success, filters } = filtersFromState;
  console.log(filters);

  useEffect(() => {
    if (searchParams.get("page") && searchParams.get("item")) {
      setCurrentPage(Number(searchParams.get("page")));
      setCurrentItem(Number(searchParams.get("item")));

      dispatch(
        listBooks(
          Number(searchParams.get("page")),
          Number(searchParams.get("item")),
          filters
        )
      );
    } else {
      dispatch(listBooks(1, 10, filters));
      setCurrentPage(1);
      setCurrentItem(10);
    }
  }, [dispatch, searchParams, filters]);

  return (
    <div>
      {loading ? (
        <Loader />
      ) : error ? (
        <Message variant="danger">{error}</Message>
      ) : headers ? (
        <div className="homeScreenBody">
          <Navigation Showcategories />
          <div className="cardRows">
            <Row data-testid="cardRow" className="mainScreen">
              {books.map((book) => (
                <Col key={book.Id} sm={12} md={6} lg={4} xl={3}>
                  <Bookcard book={book} />
                </Col>
              ))}
            </Row>
          </div>
          <div className="paginateItem">
            {headers && (
              <Paginate
                page={currentPage}
                pages={3}
                nextPage={headers["x-nextpage"]}
                prevPage={headers["x-prevpage"]}
                item={currentItem}
              />
            )}
          </div>
        </div>
      ) : (
        <Loader />
      )}
    </div>
  );
}

export default HomeScreen;
