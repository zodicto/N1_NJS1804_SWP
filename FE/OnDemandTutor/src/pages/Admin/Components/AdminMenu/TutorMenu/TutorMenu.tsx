import { Link } from "react-router-dom";
import { path } from "../../../../../constant/path";
interface props{
    list:string;
    con:string;
    rej:string;
}
export default function TurorMenu({
    list,
    con,
    rej
}:props) {
    const ListStyle = list === 'list' ? 'p-5 bg-blue-500' : 'hover:bg-blue-400 p-5';//p-5 hover:bg-blue-500
    const conStyle = con === 'con' ? 'p-5 bg-blue-500' : 'hover:bg-blue-400 p-5';
    const rejStyle = rej === 'rej' ? 'bg-blue-500 p-5' : 'hover:bg-blue-400 p-5';
    return ( <>
        <div className="flex bg-transparent justify-center  border-2 shadow-black rounded-2xl shadow-sm">
            <Link to={path.Admin.tutorList}><button className={ListStyle}>Danh sách gia sư</button></Link>     
            <div className="p-5">||</div>
            <Link to={path.Admin.confirmProfileRegisterTT}><button className={conStyle}>Đơn yêu cầu thành giảng viên</button></Link>
            {/* <div className="p-5">||</div>
            <Link to={path.Admin.rejectProfileRegisterTT}><button className={rejStyle}>Đơn đã từ chối</button></Link>  */}
            <div></div>
        </div>
        
    </> );
// import { Link } from 'react-router-dom'
// import { path } from '../../../../../constant/path'

// function TurorMenu() {
//   return (
//     <>
//       <div className='flex bg-slate-400 rounded-lg justify-center mr-10 ml-10'>
//         <Link to={path.Admin.tutorList}>
//           <button className='p-5 hover:bg-blue-500'>
//             Danh sách giảng viên
//           </button>
//         </Link>
//         <div className='p-5'>||</div>
//         <Link to={path.Admin.confirmProfileRegisterTT}>
//           <button className='p-5 hover:bg-blue-500'>
//             Đơn yêu cầu thành giảng viên
//           </button>
//         </Link>
//         <div className='p-5'>||</div>
//         <Link to={path.Admin.rejectProfileRegisterTT}>
//           <button className='p-5 hover:bg-blue-500'>Đơn đã từ chối</button>
//         </Link>
//         <div></div>
//       </div>
//     </>
//   )
}
 TurorMenu
