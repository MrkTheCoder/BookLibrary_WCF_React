import axios from "axios";
import {
  BORROWER_LIST_FAIL,
  BORROWER_LIST_REQUEST,
  BORROWER_LIST_SUCCESS,
} from "../constants/borrowersConstants";

export const listBorrowers = () => async (dispatch) => {
  try {
    dispatch({ type: BORROWER_LIST_REQUEST });
    const { data } = await axios.get("http://localhost:51204/api/borrowers");
    dispatch({ type: BORROWER_LIST_SUCCESS, payload: data });
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
