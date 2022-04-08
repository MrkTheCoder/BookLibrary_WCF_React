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
      <Table responsive striped bordered hover variant="light">
        <thead>
          <tr>
            <th>#</th>
            {Array.from({ length: 12 }).map((_, index) => (
              <th key={index}>Table heading</th>
            ))}
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>1</td>
            {Array.from({ length: 12 }).map((_, index) => (
              <td key={index}>Table cell {index}</td>
            ))}
          </tr>
          <tr>
            <td>2</td>
            {Array.from({ length: 12 }).map((_, index) => (
              <td key={index}>Table cell {index}</td>
            ))}
          </tr>
          <tr>
            <td>3</td>
            {Array.from({ length: 12 }).map((_, index) => (
              <td key={index}>Table cell {index}</td>
            ))}
          </tr>
        </tbody>
      </Table>
    </div>
  );
}

export default Borrowers;
