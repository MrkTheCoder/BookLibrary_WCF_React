import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import {
  Card,
  Container,
  Col,
  ListGroup,
  Button,
  Table,
} from "react-bootstrap";
import { listBorrowers } from "../actions/borrowersActions";
import Loader from "../components/Loader";
import Message from "../components/Message";

function Borrowers() {
  const dispatch = useDispatch();
  const borrowersFromState = useSelector((state) => state.borrowers);
  const { error, loading, borrowers } = borrowersFromState;

  useEffect(() => {
    borrowers.length == 0 && dispatch(listBorrowers());
    console.log(borrowers);
  }, [dispatch]);
  return (
    <div>
      {loading ? (
        <Loader />
      ) : error ? (
        <Message variant="danger">error</Message>
      ) : (
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
      )}
    </div>
  );
}

export default Borrowers;
