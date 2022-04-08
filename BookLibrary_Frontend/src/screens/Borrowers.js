import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Table } from "react-bootstrap";
import { useSearchParams } from "react-router-dom";

import { listBorrowers } from "../actions/borrowersActions";
import Loader from "../components/Loader";
import Message from "../components/Message";
import Paginate from "../components/Paginate";
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
                <th>FirstName</th>
                <th>LastName</th>
                <th>MiddleName</th>
                <th>Username</th>
                <th>TotalBorrows</th>
                <th>Email</th>
                <th>PhoneNo</th>
                <th>RegistrationDate</th>
                <th>Gender</th>
              </tr>
            </thead>
            <tbody>
              {borrowers.map((borrower) => (
                <tr key={borrower.username}>
                  <td>Profile picture</td>
                  <td>{borrower.FirstName}</td>
                  <td>{borrower.LastNanme}</td>
                  <td>{borrower.MiddleName}</td>
                  <td>{borrower.Username}</td>
                  <td>{borrower.TotalBorrows}</td>
                  <td>{borrower.Email}</td>
                  <td>{borrower.PhoneNo}</td>
                  <td>{borrower.RegistrationDate}</td>
                  <td>{borrower.Gender}</td>
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
