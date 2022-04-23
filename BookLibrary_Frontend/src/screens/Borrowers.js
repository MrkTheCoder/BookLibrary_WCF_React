import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Button, Table, Image } from "react-bootstrap";
import { useSearchParams, useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";

import { listBorrowers } from "../actions/borrowersActions";
import Loader from "../components/Loader";
import Message from "../components/Message";
import Paginate from "../components/Paginate";
import { LinkContainer } from "react-router-bootstrap";
import Navigation from "../components/Navigation";
function Borrowers() {
  const dispatch = useDispatch();
  const borrowersFromState = useSelector((state) => state.borrowers);
  const { error, loading, borrowers, headers } = borrowersFromState;
  const [currentPage, setCurrentPage] = useState(1);
  const [currentItem, setCurrentItem] = useState(10);
  const [searchParams, setSearchParams] = useSearchParams();
  const filtersFromState = useSelector((state) => state.filters);

  const { success, filters } = filtersFromState;
  const history = useNavigate();

  useEffect(() => {
    if (searchParams.get("page") && searchParams.get("item")) {
      setCurrentPage(Number(searchParams.get("page")));
      setCurrentItem(Number(searchParams.get("item")));

      dispatch(
        listBorrowers(
          Number(searchParams.get("page")),

          filters,
          headers
        )
      );
    } else {
      history(`?page=${1}&item=${filters ? filters.item : 10}`);
      setCurrentPage(Number(searchParams.get("page")));
      setCurrentItem(Number(searchParams.get("item")));
    }
  }, [dispatch, searchParams, filters]);
  return (
    <div>
      {loading ? (
        <Loader />
      ) : error ? (
        <Message variant="danger">error</Message>
      ) : (
        <div>
          <Navigation showItems />
          <Table
            responsive
            striped
            bordered
            hover
            variant="light"
            className="table-sm"
          >
            <thead>
              <tr>
                <th>Profile picture</th>
                <th>Name</th>

                <th>Username</th>

                <th>Email</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {borrowers.map((borrower) => (
                <tr key={borrower.username}>
                  <td>
                    <Image
                      src={borrower.ImageLink}
                      style={{ "max-width": "50px" }}
                    />
                  </td>
                  <td>
                    {borrower.FirstName} {borrower.MiddleName}{" "}
                    {borrower.LastName}
                  </td>

                  <td>{borrower.Username}</td>

                  <td>{borrower.Email}</td>
                  <td>
                    <LinkContainer to={`/admin/borrowers/${borrower.Email}`}>
                      <Button>
                        <i className="fas 	fa-ellipsis-h"></i>
                      </Button>
                    </LinkContainer>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>

          <div className="paginateItem">
            {headers && (
              <Paginate
                totalItems={headers["x-totalitems"]}
                nextPage={headers["x-nextpage"]}
                prevPage={headers["x-prevpage"]}
              />
            )}
          </div>
        </div>
      )}
    </div>
  );
}

export default Borrowers;
