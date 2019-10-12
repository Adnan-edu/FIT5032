/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package AnimalSaverV5.restful.service;

import java.util.Set;
import javax.ws.rs.core.Application;

/**
 *
 * @author ridwanurrahman
 */
@javax.ws.rs.ApplicationPath("webresources")
public class ApplicationConfig extends Application {

    @Override
    public Set<Class<?>> getClasses() {
        Set<Class<?>> resources = new java.util.HashSet<>();
        addRestResourceClasses(resources);
        return resources;
    }

    /**
     * Do not modify addRestResourceClasses() method.
     * It is automatically populated with
     * all resources defined in the project.
     * If required, comment out calling this method in getClasses().
     */
    private void addRestResourceClasses(Set<Class<?>> resources) {
        resources.add(AnimalSaverV5.restful.service.AnimalFacadeREST.class);
        resources.add(AnimalSaverV5.restful.service.AnimalFamilyFacadeREST.class);
        resources.add(AnimalSaverV5.restful.service.SequenceFacadeREST.class);
        resources.add(AnimalSaverV5.restful.service.UsersFacadeREST.class);
    }
    
}
