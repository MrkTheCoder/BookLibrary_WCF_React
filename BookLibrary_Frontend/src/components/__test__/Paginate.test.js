import React from "react";
import Paginate from "../Paginate";
import { render } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import { MemoryRouter } from "react-router-dom";


const renderEL = (data) =>{
  const element = render(
    <MemoryRouter>
      <Paginate totalItems={data.totalItems} nextPage={data.nextPage} />
    </MemoryRouter>
  );
  return element
}
test("Element renders without errors", () => {
  const data = {
    totalItems: 21,
    nextPage: "?page=2&item=10",
  };
  const { getByTestId } = renderEL(data)

});

describe('we are on the first page,there are more than 3 pages' , () =>{
  const data ={
    totalItems: 50,
    nextPage: "?page=2&item=10",
  }
  
  test('prev page and last page should be disabled', () => {
    const {getByTestId,getByText} = renderEL(data)
    


    const lastPage = getByText('First')
    const prevPage = getByText('Previous')

    expect(lastPage.className).toBe('visually-hidden')
    expect(prevPage.className).toBe('visually-hidden')

  })

  test('page 1 is shown with correct link', () =>{
    const {getByTestId,getByText} = renderEL(data)
    
   
  
    const currentPAge = getByTestId("firstPageLink");
  
  
  
    expect(currentPAge.closest('a')).toHaveAttribute('href',"/?page=1&item=10");
    
    })
    test('page 2 is are shown with correct link', () =>{
      const {getByTestId,getByText} = renderEL(data)
      
     
    
      
      const secoundPage = getByTestId('1pageAfterLink')
      
    
    
      
      expect(secoundPage.closest('a')).toHaveAttribute('href',"/?page=2&item=10");
      
      })
      test('page 3 is shown with correct link', () =>{
        
        const {getByTestId,getByText} = renderEL(data)
       
      
        
        const thirdPage = getByTestId("2pageAfterLink")
      
      
        
        expect(thirdPage.closest('a')).toHaveAttribute('href',"/?page=3&item=10");
        })
          
})