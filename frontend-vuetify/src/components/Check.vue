<template>
    <h1> &nbsp;Employee &nbsp;Report&nbsp; Page</h1>
<form @submit.prevent="submitForm" v-if="!formSubmitted">
      <span>Select Project</span><br>
        <select v-model="selectedProject">
                    <option v-for="proj in projects" :value="proj">{{proj}}</option>
        </select>
      <br>
      <span>Select Employee</span><br>
      <select v-model="selectedEmployee">
                    <option v-for="emp in employees" :value="emp">{{emp}}</option>
      </select>
      <br>
      <span>Start Time</span><br>
      <input v-model=start type="datetime-local" id="start" name="start">
      <br>
      <span>End Time</span><br>
      <input v-model=end type="datetime-local" id="end" name="end">
      <br>
      <input 
        class="submit" 
        type="submit" 
        value="Submit"
      >
    </form>
    <div v-if="formSubmitted" id = "reportFinal">
      <!-- <span> Final Report </span> -->
      <div>
      <h4> Employee Name</h4>
    {{ selectedEmployee }}
    </div>
      <div>
      <h4> Project Name</h4>
    {{ selectedProject }}
    </div>
      <div>
      <h4>Start Time</h4>
      <tr v-for="s in startArray" :value="s"><td>{{s}}</td></tr>
      </div>
      <div>
      <h4>End Time</h4>
      <tr v-for="s in endArray" :value="s"><td>{{s}}</td></tr>
    </div>
    <div>
      <h4>Total Time</h4>
      <tr v-for="s in totalTime" :value="s"><td>&nbsp;&nbsp;&nbsp;{{Math.floor(s/3600)}} : {{(s%3600)/60}}</td></tr>
    </div>
    <div>
      <h4>Work Description</h4>
      <tr v-for="s in work" :value="s"><td>{{s}}</td></tr>
    </div>
    
    </div>
    <div id = "totalTime" v-if="formSubmitted">  
    Total Time : {{(Math.floor(totalTime.reduce((a, b) => a + b, 0)/3600))}} : {{(totalTime.reduce((a, b) => a + b, 0)%3600)/60}}
    <!-- convert total time to hours -->

</div>
    
</template>
<script>
 import axios from "axios";
 export default {
     data() {
         return {
             projects: [],
             selectedProject: "",
             employees: [],
             selectedEmployee: "",
             errors: [],
             start: "",
             end: "",
             formSubmitted: false,
             startArray: [],
             endArray : [],
             totalTime : [],
             work: [],
             second: 0
         };
     },
     methods: {
      submitForm: async function () {
        await axios
             .get(`http://localhost:5160/projectitems`)
             .then((response) => {
                
                    const userStartTime = new Date(this.start);
                    const userEndTime = new Date(this.end);
                    if (userStartTime > userEndTime) {
                        alert("Start time cannot be after end time");
                        return;
                    }
                 // JSON responses are automatically parsed.
                 response.data.forEach((element) => {
                    const elementStartTime = new Date(element.startTime);
                    console.log(elementStartTime);
                    const elementEndTime = new Date(element.endTime);
                    console.log(elementEndTime);
                        if (element.name == this.selectedProject && element.employeeName == this.selectedEmployee) {
                            if (userStartTime <= elementStartTime && userEndTime >= elementEndTime) {
                                this.startArray.push(element.startTime);
                                this.endArray.push(element.endTime);
                                this.totalTime.push((elementEndTime - elementStartTime)/1000);
                                this.work.push(element.workDesc);
                            }
                        }
                 });
             })
             .catch((e) => {
                 this.errors.push(e);
             });
             
             if (this.startArray.length == 0) {
                     alert("No work found for this employee on this project during this time");
                     return;
                 }
        this.formSubmitted = true
      }
    },
    

     // Pulls projects when the component is created.
     created() {
         axios
         .get(`http://localhost:5160/projectitems`)
             .then((response) => {
                 // JSON responses are automatically parsed
                 response.data.forEach((project) => {
                  if (!this.employees.includes(project.employeeName))
                     this.employees.push(project.employeeName);
                  if (!this.projects.includes(project.name))
                     this.projects.push(project.name);
                 });
                 console.log(this.projects);
             })
             .catch((e) => {
                 this.errors.push(e);
             });
     }
 };
</script>





<style>
h1 {
    /* margin-bottom: 50px; */
    background: #f2f2f2;
    color: rgb(135, 135, 135);
    font-weight: 500;
    padding-bottom: 60px;
}

body {
    /* font-family: "Open Sans", sans-serif; */
    background: #f2f2f2;
    margin-top: 80px;
    /* margin-bottom: 90px; */
    /* padding: 100px; */
    display: flex;
    justify-content: center;
    align-items: center;
    /* max-height: 100vh; */
}
form {
    align-items: center;
    padding: 10px;
    padding-left: 50px;
    padding-right: 50px;
    border: 2px solid black;
    border-radius: 15px;
    display: flex;
    flex-direction: column;
    justify-content: center;
  }


form > select {
    align-items: center;
    width: 240px;
    height: 32px;
    border: 2px solid #222;
    border-radius: 5px;
    margin: 10px;
}
form > input > option {
    
    align-items: center;
    width: 120px;
    border: 2px solid #222;
    border-radius: 5px;
    margin: 10px;
}
input 
{
    padding: 4px 8px;
    margin: 4px;
}

  span {
    font-size: 18px;
    margin: 4px;
    font-weight: 500;
    margin-left: 12px;
  }

  .submit {
    font-size: 15px;
    color: white;
    background: #f2f2f2;
    padding: 6px 12px;
    border: none;
    margin-left: 20px;
    cursor: pointer;
    border-radius: 5px;
    border: 2px solid black;
  }
#start {
    border: 2px solid #222;
    border-radius: 5px;
}
#end {
    border: 2px solid #222;
    border-radius: 5px;
}

#work {
    border: 2px solid #222;
    border-radius: 5px;
    width: 240px;
    height: 80px;
}

#reportFinal {
    align-items: center;
    width: 1000px;
    padding: 10px;
    padding-left: 50px;
    padding-right: 50px;
    /* border: 2px solid black; */
    border-radius: 15px;
    display: flex;
    /* flex-direction: column; */
    /* justify-content: center; */
    justify-content: space-between;

  }

  #totalTime {
    margin-top: 50px;
    margin-right: 80px;
    text-align: right;
    font-size: large;
    font-weight: 500;

  }

</style>