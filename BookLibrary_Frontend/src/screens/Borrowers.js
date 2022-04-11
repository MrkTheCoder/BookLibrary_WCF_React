import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Button, Table, Image } from "react-bootstrap";
import { useSearchParams } from "react-router-dom";
import { Link } from "react-router-dom";

import { listBorrowers } from "../actions/borrowersActions";
import Loader from "../components/Loader";
import Message from "../components/Message";
import Paginate from "../components/Paginate";
import { LinkContainer } from "react-router-bootstrap";
function Borrowers() {
  const dispatch = useDispatch();
  const borrowersFromState = useSelector((state) => state.borrowers);
  const { error, loading, borrowers, headers } = borrowersFromState;
  const [currentPage, setCurrentPage] = useState(1);
  const [currentItem, setCurrentItem] = useState(10);
  const [searchParams, setSearchParams] = useSearchParams();

  useEffect(() => {
    setCurrentPage(
      Number(searchParams.get("page")) ? Number(searchParams.get("page")) : 1
    );
    setCurrentItem(
      Number(searchParams.get("item")) ? Number(searchParams.get("item")) : 10
    );
    dispatch(
      listBorrowers(
        Number(searchParams.get("page")),
        Number(searchParams.get("item"))
      )
    );
  }, [dispatch, searchParams]);
  return (
    <div>
      {loading ? (
        <Loader />
      ) : error ? (
        <Message variant="danger">error</Message>
      ) : (
        <div>
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
                    {borrower.LastNanme}
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
                page={currentPage}
                pages={3}
                nextPage={headers["x-nextpage"]}
                prevPage={headers["x-prevpage"]}
                item={Number(currentItem)}
              />
            )}
          </div>
        </div>
      )}
    </div>
  );
}

export default Borrowers;
