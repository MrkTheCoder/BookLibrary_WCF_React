import React, { useState, useEffect } from "react";
import { Pagination, Form, Button } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { useNavigate } from "react-router-dom";

import "./Paginate.css";

function Paginate({ totalItems, page, nextPage, prevPage, item }) {
  const [CustomPage, setCustomPage] = useState();
  const [CurrentPage, setCurrentPage] = useState(null);

  const history = useNavigate();

  let totalPage = Math.ceil(totalItems / item);
  const currentPage = 0;

  console.log(currentPage);

  const customPageHandler = (e) => {
    console.log(totalPage);
    e.preventDefault();

    if (CustomPage && CustomPage >= 1 && CustomPage <= totalPage) {
      history(`?page=${CustomPage}&item=${item}`);
    }
  };
  return (
    <div className="mainPaginationBody">
      <Pagination className="pagination">
        <LinkContainer to={`${prevPage}`}>
          <Pagination.Prev className={prevPage ? "" : "disabled"} />
        </LinkContainer>
        {prevPage && (
          <LinkContainer to={`?page=${page - 1}&item=${item}`}>
            <Pagination.Item>{page - 1}</Pagination.Item>
          </LinkContainer>
        )}

        <LinkContainer to={`?page=${page}&item=${item}`}>
          <Pagination.Item active={page === page}>{page}</Pagination.Item>
        </LinkContainer>

        {nextPage && (
          <LinkContainer to={`?page=${page + 1}&item=${item}`}>
            <Pagination.Item active={page + 1 === page}>
              {page + 1}
            </Pagination.Item>
          </LinkContainer>
        )}

        <LinkContainer to={`${nextPage}`}>
          <Pagination.Next className={nextPage ? "" : "disabled"} />
        </LinkContainer>
      </Pagination>
      <Form className="customPage" onSubmit={customPageHandler}>
        <Form.Group>
          <Form.Control
            onChange={(e) => setCustomPage(e.target.value)}
            placeholder="Enter page number..."
          />
        </Form.Group>
        <Button type="submit">GO!</Button>
      </Form>
    </div>
  );
}

export default Paginate;
