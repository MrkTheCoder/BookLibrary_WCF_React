import axios from "axios";
import {
  BORROWER_LIST_FAIL,
  BORROWER_LIST_REQUEST,
  BORROWER_LIST_SUCCESS,
  //
  BORROWER_DETAILS_REQUEST,
  BORROWER_DETAILS_FAIL,
  BORROWER_DETAILS_SUCCESS,
} from "../constants/borrowersConstants";

export const listBorrowers = (allFilters) => async (dispatch) => {
  try {
    dispatch({ type: BORROWER_LIST_REQUEST });

    const { data, headers } = await axios.get(
      `http://localhost:51202/api/BorrowerManager/borrowers`,
      { params: allFilters }
    );
    dispatch({
      type: BORROWER_LIST_SUCCESS,
      payload: data,
      headers: headers,
    });
  } catch (error) {
    dispatch({
      type: BORROWER_LIST_FAIL,
      payload:
        error.response && error.response.data.Message
          ? error.response.data.Message
          : error.message,
    });
  }
};
export const borrowerDetails = (email) => async (dispatch) => {
  try {
    dispatch({ type: BORROWER_DETAILS_REQUEST });
    const { data } = await axios.get(
      `http://localhost:51202/api/BorrowerManager/borrowers/${email}`
    );
    dispatch({
      type: BORROWER_DETAILS_SUCCESS,
      payload: data,
    });
  } catch (error) {
    dispatch({
      type: BORROWER_DETAILS_FAIL,
      payload:
        error.response && error.response.data.Message
          ? error.response.data.Message
          : error.message,
    });
  }
};
