import { Link } from "react-router-dom";
import { path } from "../../../../../constant/path";

interface props{
    list:string;
    con:string;
    rej:string;
}
function SessionMenu({
    list,
    con,
    rej
}:props) {
    const ListStyle = list === 'list' ? 'p-5 bg-blue-500' : 'hover:bg-blue-400 p-5';//p-5 hover:bg-blue-500
    const conStyle = con === 'con' ? 'p-5 bg-blue-500' : 'hover:bg-blue-400 p-5';
    const rejStyle = rej === 'rej' ? 'bg-blue-500 p-5' : 'hover:bg-blue-400 p-5';
    return ( <>
        <div className="flex bg-slate-400 rounded-lg justify-center mr-10 ml-10">
            <Link to={path.Admin.tutorList}><button className={ListStyle}>Thông tin phiên học</button></Link>     
            <div className="p-5">||</div>
            <Link to={path.Admin.confirmProfileRegisterTT}><button className={conStyle}>xxx</button></Link>
            <div className="p-5">||</div>
            <Link to={path.Admin.rejectProfileRegisterTT}><button className={rejStyle}>xx</button></Link> 
            <div></div>
        </div>
        
    </> );
}

export default SessionMenu;