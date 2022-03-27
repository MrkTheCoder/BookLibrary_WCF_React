import axios from "axios";
import {
  BOOK_LIST_REQUEST,
  BOOK_LIST_SUCCESS,
  BOOK_LIST_FAIL,
  //
  BOOK_DETAILS_REQUEST,
  BOOK_DETAILS_SUCCESS,
  BOOK_DETAILS_FAIL,
} from "../constants/bookConstants";

export const listBooks =
  (page = 1, item = 10) =>
  async (dispatch) => {
    try {
      dispatch({ type: BOOK_LIST_REQUEST });
      console.log(item);
      const { data, headers } = await axios.get(
        `http://localhost:51202/api/books?page=${page}&item=${item}`
      );

      dispatch({
        type: BOOK_LIST_SUCCESS,
        payload: data,
        headers: headers,
      });
    } catch (error) {
      dispatch({
        type: BOOK_LIST_FAIL,
        payload:
          error.response && error.response.data.Message
            ? error.response.data.Message
            : error.message,
      });
    }
  };

export const bookDetails = (Isbn) => async (dispatch) => {
  try {
    dispatch({ type: BOOK_DETAILS_REQUEST });
    const { data } = await axios.get(`http://localhost:51202/api/book/${Isbn}`);
    dispatch({ type: BOOK_DETAILS_SUCCESS, payload: data });
  } catch (error) {
    dispatch({
      type: BOOK_DETAILS_FAIL,
      payload:
        error.response && error.response.data.Message
          ? error.response.data.Message
          : error.message,
    });
  }
};
