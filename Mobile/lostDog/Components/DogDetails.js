import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';

const {width, height} = Dimensions.get("screen")


export default class DogDetails extends React.Component {
  constructor(props) {
    super(props);
   }

  toUri = (picture) =>{
    return "data:" + picture.fileType+";base64,"+picture.data
  }

  SetDogAsFound=(id)=>{
    var token = 'Bearer ' + this.props.token 
    //console.log("SetDogAsFound" + token + " id: " + this.props.item.id)
    fetch(this.props.Navi.URL + 'lostdogs/'+this.props.item.id+'/found', {
        method: 'PUT', 
        headers: {
            'Content-Type': 'application/json',
            'Accept': '*/*',
            'Authorization': token,
        },
    })
    .then(response => {
      if (response.status == 404 || response.status == 401) {
          return null;
      }
      else if (response.status == 200) {
          return response.json();
        }
      else{
        return null;
      }
      })
      .then(responseData => {
        if (responseData != null) 
        {
          console.log("Succedd")
        }
        else{
          this.PutDataFailed()
        }
      })
      .catch(this.PutDataFailed())
      .finally(()=>{this.props.Navi.swtichPage(3)})
  }
  PutDataFailed=()=>{
    console.log("Set data as found Failed");
  }

  ButtonRound=(l1,l2,callback)=>{
      return(
          <TouchableOpacity onPress={callback}>
            <View style={{ backgroundColor: 'green', margin: 3, borderRadius: 60, height: 73,width: 73, borderColor: 'green', justifyContent: 'center'}}>
              <View style={{alignSelf: 'center', padding: 5}}>
                  <Text style={{color: 'white', fontSize: 13}}>{l1}</Text>
                  {l2!=null? 
                  <Text style={{color: 'white', fontSize: 13}}>{l2}</Text>:null}
              </View>
            </View>
          </TouchableOpacity>
      )
  }

  getFoundInfo=()=>{
    return(
      <View>
        <Text style={{fontSize: 13, margin: 5}}>
        We are thrilled to hear{'\n'}
        your puppy is found!{'\n'}
        Don't forget to hug{'\n'}
        him from us and remember {'\n'}
        to help others by posting{'\n'}
        info everytime{'\n'}
        you see
        <Text style={{fontWeight: 'bold'}}> Lost Dog</Text>
        </Text>
      </View>
    )
  }
  render(){
    return(
      <View>
      {
        this.props.item!=null?
        <View style={styles.content}>
            <Text style={{fontWeight: 'bold', fontSize: 30}}>{this.props.item.name}</Text>
            <View style={{flex: 1,flexDirection: 'row'}}>
              {/*Picture*/
                this.props.item.picture.data!=null? 
                <Image source={{uri: this.toUri(this.props.item.picture)}} style={styles.dogPic}/> 
                : <View/>
              }
              {
                this.props.item.isFound==false?
                <View style={{alignSelf: 'flex-end', marginLeft: 80}}>
                  {this.ButtonRound('   Set dog ',' as  found ',this.SetDogAsFound)}
                  {this.ButtonRound('Edit Dog',null,null)}
                  {this.ButtonRound('Love Dog',null,null)}
                </View>:
                this.getFoundInfo(this.props.item.isFound)
              }
            </View>
            <Text style={styles.infoText}>Lost on {this.props.item.dateLost.split("T")[0]}</Text>
            <Text style={styles.infoText}>around time: {this.props.item.dateLost.split("T")[1]}</Text>
            <Text style={styles.infoText}>In the city: {this.props.item.location.city}, {this.props.item.location.district}</Text>
            <Text style={styles.infoText}>breed: {this.props.item.breed}</Text>
            <Text style={styles.infoText}>age: {this.props.item.age}</Text>
            <Text style={styles.infoText}>size: {this.props.item.size}</Text>
            <Text style={styles.infoText}>color: {this.props.item.color}</Text>
            <Text style={styles.infoText}>specialMark: {this.props.item.specialMark}</Text>
            <Text style={styles.infoText}>hairLength: {this.props.item.hairLength}</Text>
            <Text style={styles.infoText}>earsType: {this.props.item.earsType}</Text>
            <Text style={styles.infoText}>tailLength: {this.props.item.tailLength}</Text>
            <Text style={styles.infoText}>behaviors: {this.props.item.hairLength}</Text>
            {
              this.props.item.behaviors.map((e)=>{<Text style={styles.infoText}>e.behvaior</Text>})
            }
        </View>
        :null
      }
      </View>
  )
  }

}


const styles = StyleSheet.create({
    infoText:{
      fontSize: 15,
      margin: 2,
    },
      content: {
        margin: 30,
        alignSelf: 'center',
        justifyContent: 'center',
      },
      text:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 10,
        color: 'black',
        textAlign: 'center',
    },
    dogPic:{
      height: 120,
      width: 120,
      resizeMode: 'contain',
      aspectRatio: 0.7, 
      borderRadius: 600,
    },
    statusBas:{

    }
});
