import React, { useState } from "react";
import { Pagination } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { Link } from "react-router-dom";
import "./Paginate.css";

function Paginate({ pages, page, nextPage = 1, prevPage = 1, item = 10 }) {
  return (
    pages > 1 && (
      <Pagination className="pagination">
        <LinkContainer to={`${prevPage}`}>
          <Pagination.Prev className={page == 1 ? "disabled" : ""} />
        </LinkContainer>
        {[...Array(pages).keys()].map((x) => (
          <LinkContainer key={x + 1} to={`?page=${x + 1}&item=${item}`}>
            <Pagination.Item active={x + 1 === page}>{x + 1}</Pagination.Item>
          </LinkContainer>
        ))}
        <LinkContainer to={`${nextPage}`}>
          <Pagination.Next className={page == pages ? "disabled" : ""} />
        </LinkContainer>
      </Pagination>
    )
  );
}

export default Paginate;
