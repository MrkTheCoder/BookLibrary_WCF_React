import React, { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { Row, Col } from "react-bootstrap";
import Bookcard from "../components/Bookcard";
import Bookcardsv2 from "../components/Bookcardsv2";
import { listBooks } from "../actions/bookActions";
import Loader from "../components/Loader";
import Message from "../components/Message";
import Paginate from "../components/Paginate";
import Navigation from "../components/Navigation";

function HomeScreen() {
  const dispatch = useDispatch();
  const bookList = useSelector((state) => state.bookList);
  const { error, loading, books, headers } = bookList;

  const [searchParams, setSearchParams] = useSearchParams();
  const filtersFromState = useSelector((state) => state.filters);
  const [currentPage, setCurrentPage] = useState();

  const { success, filters } = filtersFromState;

  const history = useNavigate();

  useEffect(() => {
    if (
      filters &&
      filters.item != searchParams.get("item") &&
      searchParams.get("item") != null
    ) {
      filters.item = searchParams.get("item");
    }
    if (searchParams.get("page") && searchParams.get("item")) {
      dispatch(
        listBooks(
          Number(searchParams.get("page")),

          filters,
          headers
        )
      );
    } else {
      history(`?page=${1}&item=${filters ? filters.item : 10}`);
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
          <Navigation Showcategories showItems />
          <div className="cardRows">
            <Row data-testid="cardRow" className="mainScreen">
              {books.map((book) => (
                <Col key={book.Id} sm={12} md={6} lg={4} xl={3}>
                  <Bookcardsv2 book={book} />
                </Col>
              ))}
            </Row>
          </div>
          <div>
            {headers && (
              <Paginate
                totalItems={headers["x-totalitems"]}
                nextPage={headers["x-nextpage"]}
                prevPage={headers["x-prevpage"]}
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
