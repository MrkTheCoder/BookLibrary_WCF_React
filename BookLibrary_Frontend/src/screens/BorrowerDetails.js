import React, { useEffect } from "react";
import { useParams } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";

import { LinkContainer } from "react-router-bootstrap";
import { Image, Button, Table } from "react-bootstrap";
import Loader from "../components/Loader";
import Message from "../components/Message";
import { BORROWER_DETAILS_RESET } from "../constants/borrowersConstants";

import { borrowerDetails } from "../actions/borrowersActions";

function BorrowerDetails() {
  const dispatch = useDispatch();
  const borrowerFromState = useSelector((state) => state.borrower);
  const { borrower, loading, error } = borrowerFromState;
  const { email } = useParams("email");

  useEffect(() => {
    if (!borrower) {
      dispatch(borrowerDetails(email));
    }
    if (borrower) {
      if (borrower.Email != email) {
        dispatch({ type: BORROWER_DETAILS_RESET });
        dispatch(borrowerDetails(email));
      }
    }
  }, [dispatch]);

  return (
    <div>
      {loading ? (
        <Loader />
      ) : error ? (
        <Message variant="danger">error</Message>
      ) : (
        <div>
          {borrower && (
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
                  <th>Phone number</th>
                  <th>TotalBorrows</th>
                  <th>Gender</th>
                  <th>Registration date</th>
                </tr>
              </thead>
              <tbody>
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
                  <td>{borrower.PhoneNo}</td>
                  <td>{borrower.TotalBorrows}</td>
                  <td>{borrower.Gender}</td>
                  <td>{borrower.RegistrationDate}</td>
                </tr>
              </tbody>
            </Table>
          )}
        </div>
      )}
    </div>
  );
}

export default BorrowerDetails;
