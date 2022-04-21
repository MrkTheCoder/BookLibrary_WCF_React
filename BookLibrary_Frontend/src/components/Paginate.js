import React, { useState, useEffect } from "react";
import { Pagination, Form, Button, Dropdown } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { useNavigate } from "react-router-dom";
import "./Paginate.css";
import getPageAndItem from "./getPageAndItem";

function Paginate({ totalItems, nextPage, prevPage }) {
  const [CustomPage, setCustomPage] = useState();
  const [item, setCurrentItem] = useState();
  const [page, SetCurrentPage] = useState();

  const history = useNavigate();

  let totalPage = Math.ceil(totalItems / item);

  useEffect(() => {
    if (nextPage) {
      const { page, item } = getPageAndItem(nextPage, "nextPage");

      SetCurrentPage(Number(page));
      setCurrentItem(Number(item));
    } else if (prevPage) {
      const { page, item } = getPageAndItem(prevPage, "prevPage");

      SetCurrentPage(Number(page));
      setCurrentItem(Number(item));
    }
  }, [nextPage, prevPage]);

  const CustomToggle = React.forwardRef(({ children, onClick }, ref) => (
    <p
      onClick={(e) => {
        e.preventDefault();
        onClick(e);
      }}
    >
      {children}
    </p>
  ));

  const customPageHandler = (e) => {
    console.log(page, item);
    e.preventDefault();

    if (CustomPage && CustomPage >= 1 && CustomPage <= totalPage) {
      history(`?page=${CustomPage}&item=${item}`);
    }
  };
  return (
    totalPage > 1 && (
      <div className="mainPaginationBody">
        <Pagination className="pagination">
          {totalPage >= 3 && (
            <LinkContainer
              data-testid="firstPageLink"
              to={`?page=${1}&item=${item}`}
            >
              <Pagination.First
                data-testid="firstPageButton"
                className={page == 1 ? "disabled" : null}
              />
            </LinkContainer>
          )}
          <LinkContainer data-testid="prevPageLink" to={`${prevPage}`}>
            <Pagination.Prev
              data-testid="prevPageButton"
              className={prevPage ? "" : "disabled"}
            />
          </LinkContainer>
          {!nextPage && page - 2 > 0 ? (
            <LinkContainer
              data-testid="2pageBeforeLink"
              to={`?page=${page - 2}&item=${item}`}
            >
              <Pagination.Item data-testid="2pageBeforeButton">
                {page - 2}
              </Pagination.Item>
            </LinkContainer>
          ) : null}
          {prevPage && (
            <LinkContainer
              data-testid="1pageBeforeLink"
              to={`?page=${page - 1}&item=${item}`}
            >
              <Pagination.Item data-testid="1pageBeforeButton">
                {page - 1}
              </Pagination.Item>
            </LinkContainer>
          )}
          <LinkContainer
            data-testid="currentPageLink"
            to={`?page=${page}&item=${item}`}
          >
            <Pagination.Item
              data-testid="currentPageButton"
              active={page === page}
            >
              {page}
            </Pagination.Item>
          </LinkContainer>
          {nextPage && (
            <LinkContainer
              data-testid="1pageAfterLink"
              to={`?page=${page + 1}&item=${item}`}
            >
              <Pagination.Item
                data-testid="1pageAfterButton"
                active={page + 1 === page}
              >
                {page + 1}
              </Pagination.Item>
            </LinkContainer>
          )}
          {!prevPage && page + 2 <= totalPage ? (
            <LinkContainer
              data-testid="2pageAfterLink"
              to={`?page=${page + 2}&item=${item}`}
            >
              <Pagination.Item data-testid="2pageAfterButton">
                {page + 2}
              </Pagination.Item>
            </LinkContainer>
          ) : null}
          {totalPage > 3 && (
            <Dropdown>
              <Dropdown.Toggle as={CustomToggle}>
                <Pagination.Ellipsis />
              </Dropdown.Toggle>
              <Dropdown.Menu as={"div"}>
                <Form className="customPage" onSubmit={customPageHandler}>
                  <Form.Group>
                    <Form.Control
                      className="w-auto"
                      onChange={(e) => setCustomPage(e.target.value)}
                      placeholder="Enter page number..."
                    />
                  </Form.Group>
                  <Button type="submit">GO!</Button>
                </Form>
              </Dropdown.Menu>
            </Dropdown>
          )}
          <LinkContainer data-testid="nextPageLink" to={`${nextPage}`}>
            <Pagination.Next
              data-testid="nextPageButton"
              className={nextPage ? "" : "disabled"}
            />
          </LinkContainer>
          {totalPage >= 3 && (
            <LinkContainer
              data-testid="lastPageLink"
              to={`?page=${totalItems}&item=${item}`}
            >
              <Pagination.Last
                data-testid="lastPageButton"
                className={page == totalPage ? "disabled" : null}
              />
            </LinkContainer>
          )}
        </Pagination>
      </div>
    )
  );
}

export default Paginate;
