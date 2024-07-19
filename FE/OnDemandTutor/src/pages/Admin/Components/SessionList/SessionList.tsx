import { useState } from "react";
import TurorMenu from "../AdminMenu/TutorMenu";
import Search from "antd/es/transfer/search";
import SessionMenu from "../AdminMenu/SessionMenu/SessionMenu";

function SessionList() {
    const [searchText, setSearchText] = useState('');
    return ( <>  
    <div>
            <div className="text-left p-4 ml-7">Quản lí gia sư</div>
            <SessionMenu
            list=""
            con=""
            rej=""
            />
            <div className="m-10 text-left border-r-black border-l-black border-t-black border-2 p-5 h-[570px] rounded-t-xl ">
                <div className="mb-5">
                    <Search
                    // inputText={searchText}
                    // placeHolder="Search đi nè"
                    // setInputValue={setSearchText}
                    // label="Q"
                    />
                </div> 
                {/* <Table
                className=""
                columns={columns}
                dataSource={data}
                pagination={{ pageSize: 6 }} 
                onChange={onChange}
                showSorterTooltip={{ target: "sorter-icon" }}
                /> */}
            </div>
            
        </div> 
    
    </>);
}

export default SessionList;