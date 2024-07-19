import { ChangeEvent, Dispatch, SetStateAction, useState } from "react"
// sử dụng search bằng cách xài useState
interface props{
    placeHolder:string
    label:string
    inputText:string
    setInputValue:Dispatch<SetStateAction<string>>;
}
function Search({
    placeHolder,
    label,
    inputText,
    setInputValue

}:props) {
    const handleChange = (e:ChangeEvent<HTMLInputElement>) => {
        const {value} = e.target;
        setInputValue(value)
    }
    return (<>
        
        <div className='search-bar'>
            <label>{label}</label>
            <input className="m-1 p-3 rounded-xl" value={inputText} onChange={handleChange} placeholder={placeHolder} />
        </div>
    </>);
}

export default Search;